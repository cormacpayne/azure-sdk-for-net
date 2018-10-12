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
using System.Linq;
using Hyak.Common;
using Microsoft.Azure.Test;

namespace Microsoft.Azure.Test
{
    public class TestEnvironment
    {
        /// <summary>
        /// The token audience for manageemnt endpoints
        /// </summary>
        public const string DefaultAudience = "https://management.core.windows.net/";

        /// <summary>
        /// The token audience for the graph endpoint
        /// </summary>
        public const string GraphAudience = "https://graph.windows.net/";

       /// <summary>
        /// The key inside the connection string for the management certificate
        /// </summary>
        public const string ManagementCertificateKey = ConnectionStringFields.ManagementCertificate;

        /// <summary>
        /// The key inside the connection string for the subscription identifier
        /// </summary>
        public const string SubscriptionIdKey = ConnectionStringFields.SubscriptionId;

        /// <summary>
        /// The key inside the connection string for the base uri
        /// </summary>
        public const string BaseUriKey = ConnectionStringFields.BaseUri;

        /// <summary>
        /// The service management endpoint name
        /// </summary>
        public const string ServiceManagementUri = ConnectionStringFields.ServiceManagementUri;

        /// <summary>
        /// The resource management endpoint name
        /// </summary>
        public const string ResourceManagementUri = ConnectionStringFields.ResourceManagementUri;

        /// <summary>
        /// The key inside the connection string for the userId identifier
        /// </summary>
        public const string UserIdKey = ConnectionStringFields.UserId;

        /// <summary>
        /// The key inside the connection string for the service principal
        /// </summary>
        public const string ServicePrincipalKey = ConnectionStringFields.ServicePrincipal;

        public const string UserIdDefault = "user@example.com";

        /// <summary>
        /// The key inside the connection string for the AADPassword identifier
        /// </summary>
        public const string AADPasswordKey = ConnectionStringFields.Password;

        /// <summary>
        /// The key in the connection string for AD Application secret
        /// </summary>
        public const string AADApplicationSecretKey = ConnectionStringFields.Password;

        public const string EnvironmentKey = ConnectionStringFields.Environment;
        public const EnvironmentNames EnvironmentDefault = EnvironmentNames.Prod;

        /// <summary>
        /// The key inside the connection string for the AAD client ID"
        /// </summary>
        public const string ClientIdKey = ConnectionStringFields.AADClientId;
        public const string ClientIdDefault = "1950a258-227b-4e31-a9cf-717495945fc2";

        /// <summary>
        /// The key inside the connection string for the token audience for management endpoints
        /// </summary>
        public const string ManagementResourceKey = ConnectionStringFields.ManagementResource;

        /// <summary>
        /// The key inside the connection string for the graph endpoint
        /// </summary>
        public const string GraphResourceKey = ConnectionStringFields.GraphResource;

        /// <summary>
        /// The key inside the connection string for the AAD Tenant
        /// </summary>
        public const string AADTenantKey = ConnectionStringFields.AADTenant;
        public const string AADTenantDefault = "common";

        /// <summary>
        /// A raw token to be used for authentication with the give subscription ID
        /// </summary>
        public const string RawToken = ConnectionStringFields.RawToken;
        public static string RawTokenDefault = Guid.NewGuid().ToString();

        private const ExecutionMode executionModeDefault = ExecutionMode.RDFE;

        public TestEndpoints Endpoints { get; set; }

        public string CustomAudience { get; set; }

        public static IDictionary<EnvironmentNames, TestEndpoints> EnvEndpoints;
        
        static TestEnvironment()
        {
            EnvEndpoints = new Dictionary<EnvironmentNames, TestEndpoints>();
            EnvEndpoints.Add(EnvironmentNames.Prod, new TestEndpoints
            {
                Name = EnvironmentNames.Prod,
                AADAuthUri = new Uri("https://login.windows.net"),
                GalleryUri = new Uri("https://gallery.azure.com/"),
                GraphUri = new Uri("https://graph.windows.net/"),
                IbizaPortalUri = new Uri("https://portal.azure.com/"),
                RdfePortalUri = new Uri("http://go.microsoft.com/fwlink/?LinkId=254433"),
                ResourceManagementUri = new Uri("https://management.azure.com/"),
                ServiceManagementUri = new Uri("https://management.core.windows.net"),
                DataLakeStoreServiceUri = new Uri("https://azuredatalakestore.net"),
                DataLakeAnalyticsJobAndCatalogServiceUri = new Uri("https://azuredatalakeanalytics.net")
            });
            EnvEndpoints.Add(EnvironmentNames.Dogfood, new TestEndpoints
            {
                Name = EnvironmentNames.Dogfood,
                AADAuthUri = new Uri("https://login.windows-ppe.net"),
                GalleryUri = new Uri("https://df.gallery.azure-test.net/"),
                GraphUri = new Uri("https://graph.ppe.windows.net/"),
                IbizaPortalUri = new Uri("http://df.onecloud.azure-test.net"),
                RdfePortalUri = new Uri("https://windows.azure-test.net"),
                ResourceManagementUri = new Uri("https://api-dogfood.resources.windows-int.net/"),
                ServiceManagementUri = new Uri("https://management-preview.core.windows-int.net"),
                DataLakeStoreServiceUri = new Uri("https://caboaccountdogfood.net"),
                DataLakeAnalyticsJobAndCatalogServiceUri = new Uri("https://konaaccountdogfood.net")
            });
            EnvEndpoints.Add(EnvironmentNames.Next, new TestEndpoints
            {
                Name = EnvironmentNames.Next,
                AADAuthUri = new Uri("https://login.windows-ppe.net"),
                GalleryUri = new Uri("https://next.gallery.azure-test.net/"),
                GraphUri = new Uri("https://graph.ppe.windows.net/"),
                IbizaPortalUri = new Uri("http://next.onecloud.azure-test.net"),
                RdfePortalUri = new Uri("https://auxnext.windows.azure-test.net"),
                ResourceManagementUri = new Uri("https://api-next.resources.windows-int.net/"),
                ServiceManagementUri = new Uri("https://managementnext.rdfetest.dnsdemo4.com"),
                DataLakeStoreServiceUri = new Uri("https://caboaccountdogfood.net"), // TODO: change once a "next" environment is published
                DataLakeAnalyticsJobAndCatalogServiceUri = new Uri("https://konaaccountdogfood.net") // TODO: change once a "next" environment is published
            });
            EnvEndpoints.Add(EnvironmentNames.Current, new TestEndpoints
            {
                Name = EnvironmentNames.Current,
                AADAuthUri = new Uri("https://login.windows-ppe.net"),
                GalleryUri = new Uri("https://current.gallery.azure-test.net/"),
                GraphUri = new Uri("https://graph.ppe.windows.net/"),
                IbizaPortalUri = new Uri("http://current.onecloud.azure-test.net"),
                RdfePortalUri = new Uri("https://auxcurrent.windows.azure-test.net"),
                ResourceManagementUri = new Uri("https://api-current.resources.windows-int.net/"),
                ServiceManagementUri = new Uri("https://management.rdfetest.dnsdemo4.com"),
                DataLakeStoreServiceUri = new Uri("https://caboaccountdogfood.net"), // TODO: change once a "Current" environment is published
                DataLakeAnalyticsJobAndCatalogServiceUri = new Uri("https://konaaccountdogfood.net") // TODO: change once a "Current" environment is published
            });
        }
        
        public TestEnvironment()
            : this(null, executionModeDefault)
        {
        }

        public TestEnvironment(IDictionary<string, string> connection, ExecutionMode mode)
        {
            // Instantiate dictionary of parameters
            RawParameters = new Dictionary<string, string>();
            // By default set env to Prod
            this.Endpoints = TestEnvironment.EnvEndpoints[EnvironmentDefault];

            if (mode == ExecutionMode.CSM)
            {
                this.BaseUri = this.Endpoints.ResourceManagementUri;
            }
            else
            {
                this.BaseUri = this.Endpoints.ServiceManagementUri;
            }
            this.ClientId = TestEnvironment.ClientIdDefault;
            this.Tenant = TestEnvironment.AADTenantDefault;
            this.ManagementResource = TestEnvironment.DefaultAudience;
            this.GraphResource = TestEnvironment.GraphAudience;

            if (connection != null)
            {
                if (connection.ContainsKey(TestEnvironment.UserIdKey))
                {
                    this.UserName = connection[TestEnvironment.UserIdKey];
                }

                if (connection.ContainsKey(TestEnvironment.ServicePrincipalKey))
                {
                    this.ServicePrincipal = connection[TestEnvironment.ServicePrincipalKey];
                }
                if (connection.ContainsKey(TestEnvironment.AADTenantKey))
                {
                    this.Tenant = connection[TestEnvironment.AADTenantKey];
                }
                if (connection.ContainsKey(TestEnvironment.SubscriptionIdKey))
                {
                    this.SubscriptionId = connection[TestEnvironment.SubscriptionIdKey];
                }
                if (connection.ContainsKey(TestEnvironment.ClientIdKey))
                {
                    this.ClientId = connection[TestEnvironment.ClientIdKey];
                }
                if (connection.ContainsKey(TestEnvironment.ManagementResourceKey))
                {
                    this.ManagementResource = connection[TestEnvironment.ManagementResourceKey];
                }
                if (connection.ContainsKey(TestEnvironment.GraphResourceKey))
                {
                    this.GraphResource = connection[TestEnvironment.GraphResourceKey];
                }

                if (connection.ContainsKey(TestEnvironment.EnvironmentKey))
                {
                    if (TestEnvironmentHelpers.ConnectionStringContainsEndpoint(connection))
                    {
                        throw new ArgumentException("Invalid connection string, can contain endpoints or environment but not both",
                            "connection");
                    }

                    var envNameString = connection[TestEnvironment.EnvironmentKey];

                    EnvironmentNames envName;
                    if(!Enum.TryParse<EnvironmentNames>(envNameString, out envName))
                    {
                        throw new Exception(
                            string.Format("Environment \"{0}\" is not valid", envNameString));
                    }

                    this.Endpoints = TestEnvironment.EnvEndpoints[envName];
                    //need to set the right baseUri
                    if (mode == ExecutionMode.CSM)
                    {
                        this.BaseUri = this.Endpoints.ResourceManagementUri;
                    }
                    else
                    {
                        this.BaseUri = this.Endpoints.ServiceManagementUri;
                    }
                }
                if (connection.ContainsKey(TestEnvironment.BaseUriKey))
                {
                    var baseUriString = connection[TestEnvironment.BaseUriKey];
                    this.BaseUri = new Uri(baseUriString);
                    if (!connection.ContainsKey(TestEnvironment.EnvironmentKey))
                    {
                        EnvironmentNames envName = LookupEnvironmentFromBaseUri(mode, baseUriString);
                        this.Endpoints = TestEnvironment.EnvEndpoints[envName];
                    }
                }
                if (connection.ContainsKey(ConnectionStringFields.AADAuthenticationEndpoint))
                {
                    this.Endpoints.AADAuthUri = new Uri(connection[ConnectionStringFields.AADAuthenticationEndpoint]);
                }
                if (connection.ContainsKey(ConnectionStringFields.GraphUri))
                {
                    this.Endpoints.GraphUri = new Uri(connection[ConnectionStringFields.GraphUri]);
                }
                if (connection.ContainsKey(ConnectionStringFields.GalleryUri))
                {
                    this.Endpoints.GalleryUri = new Uri(connection[ConnectionStringFields.GalleryUri]);
                }
                if (connection.ContainsKey(ConnectionStringFields.IbizaPortalUri))
                {
                    this.Endpoints.IbizaPortalUri = new Uri(connection[ConnectionStringFields.IbizaPortalUri]);
                }
                if (connection.ContainsKey(ConnectionStringFields.RdfePortalUri))
                {
                    this.Endpoints.RdfePortalUri = new Uri(connection[ConnectionStringFields.RdfePortalUri]);
                }
                if (connection.ContainsKey(ConnectionStringFields.DataLakeStoreServiceUri))
                {
                    this.Endpoints.DataLakeStoreServiceUri = new Uri(connection[ConnectionStringFields.DataLakeStoreServiceUri]);
                }
                if (connection.ContainsKey(ConnectionStringFields.DataLakeAnalyticsCatalogAndJobServiceUri))
                {
                    this.Endpoints.DataLakeAnalyticsJobAndCatalogServiceUri = new Uri(connection[ConnectionStringFields.DataLakeAnalyticsCatalogAndJobServiceUri]);
                }
                if (connection.ContainsKey(ConnectionStringFields.Audience))
                {
                    CustomAudience = connection[ConnectionStringFields.Audience];
                }
                RawParameters = connection;
            }

        }

        private bool CustomUri = false;
        private Uri _BaseUri;

        public Uri BaseUri
        {
            get
            {
                return this._BaseUri;
            }

            set
            {
                this.CustomUri = true;
                this._BaseUri = value;
            }
        }
        
        public AuthorizationContext AuthorizationContext { get; set; }

        public string UserName { get; set; }

        public string ServicePrincipal { get; set; }

        public string Tenant { get; set; }

        public string ClientId { get; set; }

        public string ManagementResource { get; set; }

        public string GraphResource { get; set; }

        public string SubscriptionId { get; set; }

        public IDictionary<string, string> RawParameters { get; set; }

        public bool UsesCustomUri()
        {
            return this.CustomUri;
        }
        
        public EnvironmentNames LookupEnvironmentFromBaseUri(ExecutionMode mode, string endpointValue)
        {
            foreach(TestEndpoints testEndpoint in EnvEndpoints.Values)
            {
                if (TestEnvironmentHelpers.MatchEnvironmentBaseUri(testEndpoint, mode, endpointValue))
                {
                    return testEndpoint.Name;
                }
            }
            return EnvironmentNames.Prod;
        }

    }
}
