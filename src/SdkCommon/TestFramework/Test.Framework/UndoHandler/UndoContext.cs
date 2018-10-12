//
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using Microsoft.Azure.Test.HttpRecorder;

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// A coordinator for tracking and undoing WAML operations.  Usage pattern is
    /// using(UndoContext.Create()){//waml stuff}
    /// You can also manually call the Dispose() or UndoAll() methods to undo all 'undoable' operations since the
    /// UndoContext was created.
    /// Call: UndoContext.Commit() to remove all undo information
    /// </summary>
    public class UndoContext : IDisposable
    {
        /// <summary>
        /// Using the singleton pattern - there can be only one undo context per AppDomain
        /// </summary>
        static UndoContext()
        {
            UndoContext.Current = new UndoContext();
        }

        /// <summary>
        /// Set up the environment to enable processing undos, but do not begin processing them until
        /// an UndoContext is specifically created by the user
        /// </summary>
        private UndoContext()
        {
            this.Active = false;
            this.UndoOperations = new Stack<UndoableRestOperation>();
            this.UndoHandlerChain = UndoHandlerFactory.CreateChain();
            this.Interceptor = UndoContextTracingInterceptor.Create(this);
            if (HttpMockServer.FileSystemUtilsObject == null)
            {
                HttpMockServer.FileSystemUtilsObject = new FileSystemUtils();
            }
        }

        //prevent multiple dispose events
        protected bool disposed = false;

        /// <summary>
        /// The interceptor used to capture and process management api calls
        /// </summary>
        internal UndoContextTracingInterceptor Interceptor
        {
            get;
            set;
        }

        /// <summary>
        /// Using the singleton pattern - there can be only one undo context in an AppDomain
        /// </summary>
        public static UndoContext Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates whether the UndoContext is active or not
        /// </summary>
        protected bool Active
        {
            get;
            private set;
        }

        /// <summary>
        /// A Chain of responsibility for looking up undo actions for operations performed in this context
        /// </summary>
        public OperationUndoHandler UndoHandlerChain
        {
            get;
            private set;
        }

        /// <summary>
        /// The set of undo actions for operations that have executed in this context
        /// </summary>
        protected Stack<UndoableRestOperation> UndoOperations
        {
            get;
            private set;
        }

        /// <summary>
        /// Return a new UndoContext
        /// </summary>
        /// <returns></returns>
        public void Start(int currentMethodStackDepth = 3)
        {
            TestUtilities.StartTest(currentMethodStackDepth);
            if (HttpMockServer.Mode != HttpRecorderMode.Playback)
            {
                this.Active = true;
                this.disposed = false;
            }
        }

        /// <summary>
        /// Return a new UndoContext
        /// </summary>
        /// <returns></returns>
        public void Start(string className, string methodName)
        {
            TestUtilities.StartTest(className, methodName);
            if (HttpMockServer.Mode != HttpRecorderMode.Playback)
            {
                this.Active = true;
                this.disposed = false;
            }
        }

        /// <summary>
        /// Stop recording and Discard all undo information
        /// </summary>
        public void Stop()
        {
            TestUtilities.EndTest();
            this.Active = false;
            this.UndoOperations.Clear();
        }


        /// <summary>
        /// Execute the operation and return
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public virtual void Execute(object instance, string method, IDictionary<string, object> parameters)
        {
            if (this.Active)
            {
                UndoableRestOperation undoOperation;
                if (this.UndoHandlerChain.Lookup(instance, method, parameters, out undoOperation))
                {
                    this.UndoOperations.Push(undoOperation);
                }
            }
        }

        /// <summary>
        /// Undo the last undoable operation
        /// </summary>
        /// <returns>true if the undo operation is successful</returns>
        public bool Undo()
        {
            if (this.UndoOperations.Count > 0)
            {
                UndoableRestOperation operation = this.UndoOperations.Pop();
                if (HttpMockServer.Mode != HttpRecorderMode.Playback)
                {
                    return operation.Undo();
                }
            }

            return true;
        }

        /// <summary>
        /// Undo all undoable operations that have been performed
        /// </summary>
        /// <returns>true if all operations were undone successfully</returns>
        public bool UndoAll()
        {
            TestUtilities.EndTest();
            this.Active = false;
            bool success = true;
            while (this.UndoOperations.Count > 0)
            {
                success = Undo() && success;
            }

            return success;
        }

        /// <summary>
        /// Dispose only if we have not previously been disposed
        /// </summary>
        /// <param name="disposing">true if we should dispose, otherwise false</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.UndoAll();
                this.disposed = true;
            }
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
