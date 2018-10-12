using System;
using System.Reflection;
using System.Threading;

namespace Microsoft.Azure.Test
{
    public enum PollingStatus
    {
        Success,
        InProgress,
        Failure
    }

    /// <summary>
    /// Abstract mechanism for performing polling operations
    /// </summary>
    public abstract class StatusPoller
    {
        /// <summary>
        /// A function that determines whether or not to retry a poll
        /// </summary>
        public Func<object, int, bool> ShouldRetry
        {
            get;
            protected set;
        }

        /// <summary>
        /// A function thet returns the polling status, given a response
        /// </summary>
        public Func<object, PollingStatus> GetPollingStatus
        {
            get;
            protected set;
        }

        /// <summary>
        /// A function that performs a single poll for status
        /// </summary>
        public Func<object, object, object> GetOperationStatus
        {
            get;
            protected set;
        }

        /// <summary>
        /// Poll for operation status, given a client, a response, and the time between polls
        /// </summary>
        /// <param name="client">The client to use for polling</param>
        /// <param name="operationData">The response - this will be the initial response for the first poll, and the pollign response for subsequent polls</param>
        /// <param name="timeout">The amount of time between polls</param>
        /// <returns>The final polling status - either a terminal status, or InProgress if the operation did not complete in the specified interval</returns>
        public PollingStatus PollForStatus(object client, object operationData, TimeSpan timeout)
        {
            int tries = 0;
            PollingStatus result = PollingStatus.InProgress;
            do 
            {
                TestUtilities.Wait(timeout);
                operationData = this.GetOperationStatus(client, operationData);
                if (operationData != null)
                {
                    result = this.GetPollingStatus(operationData);
                }
                else
                {
                    result = PollingStatus.Failure;
                }
            }
            while( result == PollingStatus.InProgress && this.ShouldRetry(client, ++tries));
            return result;
        }
    }

    /// <summary>
    /// A class for polling status for long-runnign operations
    /// </summary>
    public class ResourceManagementStatusPoller : StatusPoller
    {
        private static bool _ShouldRetry(int maxTries, object client, int tries)
        {
            return tries < maxTries;
        }

        private static object _GetOperationStatus(object client, object statusResponse)
        {
            Type clientType = client.GetType();
            MethodInfo pollingMethod = clientType.GetMethod("GetLongRunningOperationStatusAsync", new Type[] {typeof(string), typeof(CancellationToken)});
            string statusLink = GetOperationStatusLink(pollingMethod, statusResponse);
            if (statusLink != null)
            {
                var pollResult = pollingMethod.Invoke(client, new object[] { statusLink, null });
                if (pollResult != null)
                {
                    return WaitForResult(pollResult);
                }
            }

            return null;
        }

        private static PollingStatus _GetPollingStatus(object statusResponse)
        {
            object status = null;
            Type responseType = statusResponse.GetType();
            PropertyInfo statusProperty = responseType.GetProperty("Status");
            if (statusProperty != null)
            {
                status = statusProperty.GetValue(statusResponse);
            }

            return ConvertResponseStatus(status);
        }

        private static string GetOperationStatusLink(MethodInfo calledMethod, object response)
        {
            string link = null;
            PropertyInfo resultProperty = response.GetType().GetProperty("Result");
            if (resultProperty != null)
            {
                response = resultProperty.GetValue(response);
            }

            PropertyInfo linkProperty = calledMethod.ReturnType.GenericTypeArguments[0].GetProperty("OperationStatusLink", typeof(string));
            if (linkProperty != null)
            {
                link = linkProperty.GetValue(response) as string;
            }

            return link;
        }

        private static object WaitForResult(object taskObject)
        {
            object response = null;
            Type taskType = taskObject.GetType();
            MethodInfo waitMethod = taskType.GetMethod("GetAwaiter", Type.EmptyTypes);
            Type awaiterType = waitMethod.ReturnType;
            MethodInfo resultMethod = awaiterType.GetMethod("GetResult", Type.EmptyTypes);
            if (waitMethod!= null && resultMethod!= null)
            {
                var awaiter = waitMethod.Invoke(taskObject, null);
                response = resultMethod.Invoke(awaiter, null);
            }

            return response;
        }

        private static PollingStatus ConvertResponseStatus(object status)
        {
            PollingStatus result = PollingStatus.Failure;
            if (string.Equals("InProgress", status.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                result = PollingStatus.InProgress;
            }
            else if (string.Equals("Succeeded", status.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                result = PollingStatus.Success;
            }

            return result;
        }

        /// <summary>
        /// Create a poller for the given maximum number of tries
        /// </summary>
        /// <param name="maxTries">maximum number of tries</param>
        /// <returns>A status poller for resource management operations</returns>
        public static ResourceManagementStatusPoller Create(int maxTries)
        {
            return new ResourceManagementStatusPoller
            {
                ShouldRetry = (client, tries) => _ShouldRetry(maxTries, client, tries),
                GetOperationStatus = (client, response) => _GetOperationStatus(client, response),
                GetPollingStatus = (response) => _GetPollingStatus(response)
            };
        }
    }
}
