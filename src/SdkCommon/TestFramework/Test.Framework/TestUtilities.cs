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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Hyak.Common;
using Microsoft.Azure;
using Microsoft.Azure.Test.HttpRecorder;

namespace Microsoft.Azure.Test
{
    public static partial class TestUtilities
    {
        /// <summary>
        /// Simply function determining retry policy - retry on any internal server error
        /// </summary>
        public static Func<HttpStatusCode, bool> RetryOnHttp500 = (s => HttpStatusCode.InternalServerError == s);

        /// <summary>
        /// Generate a name to be used in azure
        /// </summary>
        /// <returns></returns>
        public static string GenerateName(string prefix = "azsmnet")
        {
            return HttpMockServer.GetAssetName(GetCurrentMethodName(2), prefix);
        }

        /// <summary>
        /// Generate a name to be used in azure
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateGuid()
        {
            return HttpMockServer.GetAssetGuid(GetCurrentMethodName(2));
        }

        /// <summary>
        /// Validate the fields of an operation response
        /// </summary>
        /// <param name="opResponse"> The operation response to validate</param>
        /// <param name="expectedStatus">The expected status code </param>
        public static void ValidateOperationResponse(AzureOperationResponse opResponse, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            Debug.Assert(opResponse.StatusCode == expectedStatus);
            Debug.Assert(opResponse.RequestId != null);
        }

        /// <summary>
        /// Retry the given action until successful or timeout.  Validator returns true if the action was successful, shouldRetry determines whether a retry is necessary / required
        /// </summary>
        /// <param name="action">The action to try until successful</param>
        /// <param name="validator">The validator to determine if the action was successful</param>
        /// <param name="timeout">The amount of time allocated for the operation to succeed</param>
        /// <param name="shouldRetry">Function to determine whether a particular http response should result in a retry</param>
        public static void RetryActionWithTimeout(Action action, Func<bool> validator, TimeSpan timeout, Func<HttpStatusCode, bool> shouldRetry)
        {
            DateTime timedOut = DateTime.Now + timeout;
            do
            {
                try
                {
                    action();
                }
                catch (CloudException exception)
                {
                    if (!shouldRetry(exception.Response.StatusCode))
                    {
                        throw;
                    }
                }
            }

            while (DateTime.Now < timedOut && !validator());
        }

        /// <summary>
        /// Run a test involving creation of a azure entity.  Test involves a set up action, a main creation action, and a cleanup action, which is retried until successful
        /// (or timed out).
        /// </summary>
        /// <param name="setupAction">An setup needed beofer the test action is performed</param>
        /// <param name="creationAction">The creation action which is the subject of test</param>
        /// <param name="cleanupAction">The action which cleans up the creation action, leaving the environment in the state it was in previous to the
        ///   test running</param>
        /// <param name="validateCleanup">The predicate which determines whteher or not cleanup was successful  ValidateCleanup assumes that there is some 
        /// validation independent of a particular response which can determine the success of the cleanup</param>
        /// <param name="cleanupTimeout">The amount of time in which to attemnpt to clean up artifcats created during the test</param>
        public static void RunEntityCreationTest(Func<bool> setupAction, Action creationAction, Action cleanupAction, Func<bool> validateCleanup, TimeSpan cleanupTimeout)
        {
            if (setupAction())
            {
                try
                {
                    creationAction();
                }
                finally
                {
                    RetryActionWithTimeout(cleanupAction, validateCleanup, cleanupTimeout, RetryOnHttp500);
                }
            }
        }

        /// <summary>
        /// Get a random name given a prefix
        /// </summary>
        /// <param name="name">The prefix for the name</param>
        /// <returns>The random name</returns>
        public static string Randomize(string name)
        {
            return name + Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        }

        /// <summary>
        /// Assert that an exception with a particular status code is thrown
        /// </summary>
        /// <param name="status">The expected status code</param>
        /// <param name="action">The action expected to throw a CloudException with the given status code</param>
        public static void AssertThrownStatusCode(HttpStatusCode status, Action action)
        {
            try
            {
                action();
                Debug.Assert(false, "Expected a CloudException with StatusCode " + status);
            }
            catch (CloudException ex)
            {
                if (ex.Response.StatusCode != status)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Execute the given action ignoring any exceptions
        /// </summary>
        /// <param name="action">The action to perform</param>
        public static void IgnoreExceptions(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Used for mthod traces - format method arguments as a string for output or tracing
        /// </summary>
        /// <param name="parameters">A dictionary representing the parameters of a method call</param>
        /// <returns>A string representation of the parameters</returns>
        public static string AsFormattedString(this IDictionary<string, object> parameters)
        {
            StringBuilder builder = new StringBuilder("(");
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Keys.Count; ++i)
                {
                    string key = parameters.Keys.ElementAt(i);
                    object value = parameters[key];
                    builder.AppendFormat("{0}:{1}", key, value);
                    if (i < parameters.Keys.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }
            }

            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Return the base directory used for mocks
        /// </summary>
        /// <returns>The path of the directory used for mocks</returns>
        public static string GetMockBaseDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SessionRecords");
        }

        /// <summary>
        /// Start a test and begin mocking, as appropriate
        /// </summary>
        public static void StartTest(int index = 2)
        {
            HttpMockServer.RecordsDirectory = GetMockBaseDirectory();
            HttpMockServer.Initialize(GetCallingClass(index), GetCurrentMethodName(index));
        }

        /// <summary>
        /// Start a test and begin mocking, as appropriate
        /// </summary>
        public static void StartTest(string className, string methodName)
        {
            HttpMockServer.RecordsDirectory = GetMockBaseDirectory();
            HttpMockServer.Initialize(className, methodName);
        }

        /// <summary>
        /// End a test, flushing mocks as appropriate
        /// </summary>
        public static void EndTest()
        {
            HttpMockServer.Flush();
        }

        /// <summary>
        /// Wait for the specified number of milliseconds unless we are in mock playback mode
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to wait</param>
        public static void Wait(int milliseconds)
        {
            Wait(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Wait for the specified span unless we are in mock playback mode
        /// </summary>
        /// <param name="timeout">The span of time to wait for</param>
        public static void Wait(TimeSpan timeout)
        {
            if (HttpMockServer.Mode != HttpRecorderMode.Playback)
            {
                Thread.Sleep(timeout);
            }
        }

        /// <summary>
        /// Get the method name of the calling method
        /// </summary>
        /// <param name="index">How deep into the strack trace to look - here we want the caller's caller.</param>
        /// <returns>The name of the declaring method</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName(int index = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(index);

            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Get the typename of the callling class
        /// </summary>
        /// <param name="index">How deep into the strack trace to look - here we want the caller's caller.</param>
        /// <returns>The name of the declaring type</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCallingClass(int index = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(index);

            return sf.GetMethod().ReflectedType.ToString();
        }

        /// <summary>
        /// Break up the connection string into key-value pairs
        /// </summary>
        /// <param name="connectionString">The connection string to parse</param>
        /// <returns>A dictionary of keys and values from the connection string</returns>
        public static IDictionary<string, string> ParseConnectionString(string connectionString)
        {
            // hacky connection string parser.  We should replace with more robust connection strign parsing
            IDictionary<string, string> settings = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(connectionString))
            {
                string[] pairs = connectionString.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    foreach (string pair in pairs)
                    {
                        string[] keyValue = pair.Split(new char[] {'='}, 2);
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        settings[key] = value;
                    }

                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException(
                        string.Format("Connection string \"{0}\" is invalid", connectionString),
                        "connectionString", ex);
                }
            }
            return settings;
        }
    }
}
