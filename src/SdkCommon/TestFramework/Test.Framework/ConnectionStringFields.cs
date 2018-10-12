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

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// Contains constant definitions for the fields that
    /// are allowed in the test connection strings.
    /// </summary>
    public static class ConnectionStringFields
    {
        /// <summary>
        /// The connection string key for token audience
        /// </summary>
        public const string Audience = "Audience";
        /// <summary>
        /// The key inside the connection string for the management certificate
        /// </summary>
        public const string ManagementCertificate = "ManagementCertificate";

        /// <summary>
        /// The key inside the connection string for the subscription identifier
        /// </summary>
        public const string SubscriptionId = "SubscriptionId";

        /// <summary>
        /// The key for the RDFE service management Url
        /// </summary>
        public const string ServiceManagementUri = "ServiceManagementUri";

        /// <summary>
        /// The resource management endpoint name
        /// </summary>
        public const string ResourceManagementUri = "ResourceManagementUri";

        /// <summary>
        /// The key inside the connection string for the base management URI
        /// </summary>
        public const string BaseUri = "BaseUri";

        /// <summary>
        /// The key inside the connection string for AD Graph URI
        /// </summary>
        public const string GraphUri = "GraphUri";

        /// <summary>
        /// The key inside the connection string for AD Gallery URI
        /// </summary>
        public const string GalleryUri = "GalleryUri";

        /// <summary>
        /// The key inside the connection string for the Ibiza Portal URI
        /// </summary>
        public const string IbizaPortalUri = "IbizaPortalUri";

        /// <summary>
        /// The key inside the connection string for the RDFE Portal URI
        /// </summary>
        public const string RdfePortalUri = "RdfePortalUri";

        /// <summary>
        /// The key inside the connection string for the DataLake FileSystem URI suffix
        /// </summary>
        public const string DataLakeStoreServiceUri = "DataLakeStoreServiceUri";

        /// <summary>
        /// The key inside the connection string for the Data Lake Analytics Catalog URI
        /// </summary>
        public const string DataLakeAnalyticsCatalogAndJobServiceUri = "DataLakeAnalyticsCatalogAndJobServiceUri";

        /// <summary>
        /// The key inside the connection string for a Microsoft ID (OrgId or LiveId)
        /// </summary>
        public const string UserId = "UserId";

        /// <summary>
        /// Service principal key
        /// </summary>
        public const string ServicePrincipal = "ServicePrincipal";

        /// <summary>
        /// The key inside the connection string for a user password matching the Microsoft ID OR
        /// The Application secret for AD service principal authentication
        /// </summary>
        public const string Password = "Password";

        /// <summary>
        /// A raw JWT token for AAD authentication
        /// </summary>
        public const string RawToken = "RawToken";
        
        /// <summary>
        /// The client ID to use when authenticating with AAD
        /// </summary>
        public const string AADClientId = "AADClientId";

        /// <summary>
        /// The token audience for management endpoints
        /// </summary>
        public const string ManagementResource = "ManagementResource";

        /// <summary>
        /// The token audience for the graph endpoint
        /// </summary>
        public const string GraphResource = "GraphResource";

        /// <summary>
        /// ENdpoint to use for AAD authentication
        /// </summary>
        public const string AADAuthenticationEndpoint = "AADAuthEndpoint";

        /// <summary>
        /// If a tenant other than common is to be used with the subscription, specifies the tenant
        /// </summary>
        public const string AADTenant = "AADTenant";

        public const string Environment = "Environment"; 
    }
}
