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
using System.Threading;
using Hyak.Common;
using Microsoft.Azure.Test;

namespace Microsoft.WindowsAzure.Management.WebSites.Testing
{
    public class WebSiteUndoHandler : ReflectingOperationUndoHandler
    {
        public WebSiteUndoHandler()
        {
            this.ClientTypeName = "WebSiteOperations";
            this.ClientTypeNamespace = "Microsoft.WindowsAzure.Management.Websites";
        }

        protected override bool TryFindUndoAction(object client, string method, IDictionary<string, object> parameters, out Action undoAction)
        {
            undoAction = null;
            switch (method)
            {
                case "CreateAsync":
                    return TryHandleCreateAsync(client, parameters, out undoAction);
                default:
                    TraceUndoNotFound(this, method, parameters);
                    return false;
            }
        }

        private bool TryHandleCreateAsync(object client, IDictionary<string, object> parameters, out Action undoAction)
        {
            undoAction = null;
            MethodInfo originalMethod = client.GetType().GetMethod("CreateAsync");
            object createParameters = parameters["parameters"];
            PropertyInfo siteNameProperty = createParameters.GetType().GetProperty("Name");
            string siteNameParameter = siteNameProperty.GetValue(createParameters) as string;
            string webSpaceName = parameters["webSpaceName"] as string;
            PropertyInfo webSiteOperationsProperty = this.GetServiceClientType(client).GetProperty("WebSites");
            MethodInfo undoMethod = webSiteOperationsProperty.PropertyType.GetMethod("DeleteAsync");
            ParameterInfo[] deleteParameters = undoMethod.GetParameters();
            if (deleteParameters.Length < 4 || deleteParameters[0].ParameterType != typeof(string) || deleteParameters[1].ParameterType != typeof(string) || deleteParameters[3].ParameterType != typeof(CancellationToken))
            {
                TraceParameterError(this, "CreateAsync", parameters);
                return false;
            }

            object undoParameters = deleteParameters[2].ParameterType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            PropertyInfo deleteServerFarmProperty = undoParameters.GetType().GetProperty("DeleteEmptyServerFarm", typeof(bool));
            PropertyInfo deleteMetricsProperty = undoParameters.GetType().GetProperty("DeleteMetrics", typeof(bool));
            PropertyInfo deleteSlotsProperty = undoParameters.GetType().GetProperty("DeleteAllSlots", typeof(bool));

            if (!string.IsNullOrEmpty(webSpaceName) && !string.IsNullOrEmpty(siteNameParameter) && undoMethod != null && 
                deleteMetricsProperty != null && deleteServerFarmProperty != null)
            {
                undoAction = () =>
                    {
                        object website = this.CreateServiceClient(client);
                        object webSiteOperations = webSiteOperationsProperty.GetValue(website);
                        undoParameters = deleteParameters[2].ParameterType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                        deleteServerFarmProperty.SetValue(undoParameters, true );
                        deleteMetricsProperty.SetValue(undoParameters, true );
                        deleteSlotsProperty.SetValue(undoParameters, true );
                        object undoTask = undoMethod.Invoke(webSiteOperations, new object[] { webSpaceName, siteNameParameter, undoParameters, null });
                        var result = undoTask.GetType().GetProperty("Result").GetValue(undoTask);
                        IDisposable disposableClient = website as IDisposable;
                        if (disposableClient != null)
                        {
                            disposableClient.Dispose();
                        }
                    };

                return true;
            }

            TraceParameterError(this, "CreateAsync", parameters);
            return false;
        }

        public override UndoableRestOperation CreateUndoableRestOperation(Action undoAction)
        {
            return new UndoableWebSiteRestOperation(undoAction);
        }

        private class UndoableWebSiteRestOperation : UndoableRestOperation
        {
            public UndoableWebSiteRestOperation(Action undo)
                : base(undo)
            {
            }

            public override bool ShouldRetry(CloudException exception, int tries)
            {
                HttpStatusCode code = exception.Response.StatusCode;
                return code != HttpStatusCode.Conflict && code != HttpStatusCode.BadRequest && code != HttpStatusCode.Forbidden && 
                    code != HttpStatusCode.Gone && code != HttpStatusCode.HttpVersionNotSupported && code != HttpStatusCode.MethodNotAllowed && 
                    code != HttpStatusCode.NotFound && tries < this.MaxRetries;
            }
        }
    }
}
