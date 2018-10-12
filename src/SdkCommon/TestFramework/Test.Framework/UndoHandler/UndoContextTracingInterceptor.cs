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
using System.Net.Http;
using Hyak.Common;

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// Tracing hooks that allow tracking opoerations for later undo
    /// </summary>
    internal class UndoContextTracingInterceptor : ICloudTracingInterceptor, IDisposable
    {
        //Track whether or not the interceptor has been disposed
        protected bool disposed = false;

        /// <summary>
        /// Track the undo context we will use to log WAML operations
        /// </summary>
        private UndoContext UndoContext
        {
            get;
            set;
        }

        /// <summary>
        /// Create an undo context tracing intercaptor and inject it into the WAML tracing sink
        /// </summary>
        /// <param name="context">The associated undo context</param>
        /// <returns>The created interceptor</returns>
        public static UndoContextTracingInterceptor Create(UndoContext context)
        {
            return new UndoContextTracingInterceptor(context);
        }

        /// <summary>
        /// Create an interceptor and place it into the cloud context
        /// </summary>
        /// <param name="context">The undo context used to track undo actions</param>
        private UndoContextTracingInterceptor(UndoContext context)
            : base()
        {
            this.UndoContext = context;
            this.InsertIntoCloudContext();
        }

        /// <summary>
        /// Insert the tracing interceptor into the WAML trace sink
        /// </summary>
        private void InsertIntoCloudContext()
        {
            RemoveFromCloudContext();
            TracingAdapter.AddTracingInterceptor(this);
        }

        /// <summary>
        /// Remove the interceptor from the trace sink
        /// </summary>
        private void RemoveFromCloudContext()
        {
            try
            {
                TracingAdapter.RemoveTracingInterceptor(this);
            }
            catch (Exception)
            {
            }
        }

        //No op - we are only interested in intercepting function calls
        public void Configuration(string source, string name, string value)
        {
        }

        /// <summary>
        /// Trace the start of a WAML operation and register the operation and any available undo action 
        /// with the currently active UndoContext
        /// </summary>
        /// <param name="invocationId">Correlation ID, unused</param>
        /// <param name="instance">The instance used to invoke the given operation</param>
        /// <param name="method">The name of the operation</param>
        /// <param name="parameters">The parameters passed to the operation</param>
        public void Enter(string invocationId, object instance, string method, IDictionary<string, object> parameters)
        {
            this.UndoContext.Execute(instance, method, parameters);
        }

        //No op: future versions could use the error information to decide an undo is necessary
        public void Error(string invocationId, System.Exception exception)
        {
        }

        //No op: future versions could use the operation return value in constructing undo operations
        public void Exit(string invocationId, object returnValue)
        {
        }

        // No op
        public void Information(string message)
        {
        }

        //No op: future versions could use the response to remove or alter undo information
        public void ReceiveResponse(string invocationId, HttpResponseMessage response)
        {
        }

        //no op
        public void SendRequest(string invocationId, HttpRequestMessage request)
        {
        }

        /// <summary>
        /// Dispose the interceptor if it has not previously been disposed
        /// </summary>
        /// <param name="disposing">Indicates whether the object should be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.RemoveFromCloudContext();
                this.disposed = true;
            }
        }

        /// <summary>
        /// Dispose the interceptor, as appropriate
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
