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
using System.Net;
using System.Reflection;
using Hyak.Common;
using Microsoft.Azure.Test;

namespace Microsoft.WindowsAzure.Management.ResourceManagement.Testing
{
    public class ResourceManagementUndoHandler : ReflectingOperationUndoHandler
    {
        public ResourceManagementUndoHandler()
        {
            this.ClientTypeName = "ResourceGroupOperations";
            this.ClientTypeNamespace = "Microsoft.Azure.Management.Resources";
        }

        protected override bool TryFindUndoAction(object client, string method, IDictionary<string, object> parameters, out Action undoAction)
        {
            undoAction = null;
            switch (method)
            {
                case "CreateOrUpdateAsync":
                    return TryHandleCreateAsync(client, parameters, out undoAction);
                default:
                    TraceUndoNotFound(this, method, parameters);
                    return false;
            }
        }

        private bool TryHandleCreateAsync(object client, IDictionary<string, object> parameters, out Action undoAction)
        {
            undoAction = null;
            string resourceGroupName = parameters["resourceGroupName"] as string;
            PropertyInfo resourceGroupsOperationsProperty = this.GetServiceClientType(client).GetProperty("ResourceGroups");
            MethodInfo undoMethod = resourceGroupsOperationsProperty.PropertyType.GetMethod("BeginDeletingAsync");
            ParameterInfo[] deleteParameters = undoMethod.GetParameters();
            if (deleteParameters.Length < 1 || deleteParameters[0].ParameterType != typeof(string) )
            {
                TraceParameterError(this, "CreateOrUpdateAsync", parameters);
                return false;
            }

            undoAction = () =>
            {
                object serviceClient = this.CreateServiceClient(client);
                object resourceGroupOperations = resourceGroupsOperationsProperty.GetValue(serviceClient);
                var response = undoMethod.Invoke(resourceGroupOperations, new object[] { resourceGroupName, null});
                PollingStatus status = ResourceManagementStatusPoller.Create(3).PollForStatus(serviceClient, response, TimeSpan.FromSeconds(15));
                TraceUndoMessage(this, "CreateOrUpdateAsync", string.Format("Result of status polls: {0}", status));
                IDisposable disposableClient = serviceClient as IDisposable;
                if (disposableClient != null)
                {
                    disposableClient.Dispose();
                }
            };
            return true;
        }

        public override UndoableRestOperation CreateUndoableRestOperation(Action undoAction)
        {
            return new ResourceUndoableRestOperation(undoAction);
        }

        private class ResourceUndoableRestOperation : UndoableRestOperation
        {
            public ResourceUndoableRestOperation(Action undoAction)
                : base(undoAction)
            {
                MaxRetries = 5;
                RetryIntervalSeconds = 15;
            }

            public override bool ShouldRetry(CloudException exception, int tries)
            {
                return base.ShouldRetry(exception, tries) && exception.Response.StatusCode != HttpStatusCode.OK && exception.Response.StatusCode != HttpStatusCode.NoContent;
            }
        }
    }
}
