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
using System.Diagnostics;
using System.Net;
using System.Threading;
using Hyak.Common;

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// Class which encapsulat6es the behavior of undoing a single rest operation with another single rest operation.
    /// Sets default retry behavior and the like.
    /// </summary>
    public class UndoableRestOperation
    {
        public const int DefaultMaxRetries = 10;
        public const int DefaultRetryIntervalSeconds = 2;

        /// <summary>
        /// Create a new undoable rest operation
        /// </summary>
        /// <param name="undo">The single rest operation that reverses the current operation</param>
        public UndoableRestOperation(Action undo)
            : this(undo, DefaultMaxRetries, DefaultRetryIntervalSeconds)
        {
        }

        /// <summary>
        /// Create a new undoable rest operation
        /// </summary>
        /// <param name="undo">The single rest operation that undoes this operation</param>
        /// <param name="retries">The maximum number of retires for the operation</param>
        /// <param name="interval">The interval between retries</param>
        public UndoableRestOperation(Action undo, int retries, int interval)
        {
            this.UndoOperation = undo;
            this.MaxRetries = retries;
            this.RetryIntervalSeconds = interval;
        }

        /// <summary>
        /// The maximum number of times the undo operation should be tries
        /// </summary>
        public int MaxRetries
        {
            get;
            set;
        }

        /// <summary>
        /// The interval between retries of the rest operation
        /// </summary>
        public int RetryIntervalSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// A function that performs the single undo operation
        /// </summary>
        public Action UndoOperation
        {
            get;
            set;
        }

        /// <summary>
        /// Virtual function determining normal condition unhder which an unsuccessful operation will be retried
        /// </summary>
        /// <param name="exception">The CloudException returned from the unsuccessful undo operation</param>
        /// <param name="tries">The number of tries so far</param>
        /// <returns>true if the operation should be retried</returns>
        public virtual bool ShouldRetry(CloudException exception, int tries)
        {
            // retry unless the resource is not found - unfortunately, many 400s should be retried
            HttpStatusCode response = exception.Response.StatusCode;
            return response != HttpStatusCode.NotFound  && tries < this.MaxRetries;
        }

        /// <summary>
        /// For undo handlers that use reflection, try to turn any received exceptions into cloud exceptions
        /// If they are not, do not retry and report an error
        /// </summary>
        /// <param name="exception">The received exception</param>
        /// <param name="tries">The number of tries so far</param>
        /// <returns></returns>
        public bool HandleException(Exception exception, int tries)
        {
            CloudException cloudException = exception as CloudException;
            if (cloudException != null)
            {
                Trace.WriteLine(string.Format("Received exception on undo: {0}", exception));
                return ShouldRetry(cloudException, tries);
            }
            else if (exception.InnerException != null)
            {
                return HandleException(exception.InnerException, tries);
            }

            Trace.WriteLine(string.Format("Received unexpected non-clud exception on undo: {0}", exception));
            return false;
        }
        /// <summary>
        /// Perform the undo operation, with rety logic
        /// </summary>
        /// <returns>True if the operation was successful, otherwise false</returns>
        public virtual bool Undo()
        {
            int tries = 0;
            int interval = this.RetryIntervalSeconds;
            bool success = false;
            bool shouldRetry = false;
            do
            {
                try
                {
                    UndoOperation();
                    success = true;
                }
                catch (Exception exception)
                {
                    shouldRetry = HandleException(exception, tries++);
                    if (shouldRetry) { Thread.Sleep(TimeSpan.FromSeconds(interval)); }
                    interval = interval * 2;
                }

            }
            while (!success && shouldRetry);
            return success;
        }

    }
}
