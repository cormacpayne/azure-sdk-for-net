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
using System.Security.Cryptography.X509Certificates;
using Hyak.Common;
using Microsoft.Azure.Test;
using Microsoft.Azure.Test.PublishSettings;

namespace Microsoft.Azure.Test
{
    public class RDFETestEnvironmentFactory : TestEnvironmentFactory
    {
        public RDFETestEnvironmentFactory() : base()
        {
        }
        /// <summary>
        /// The default name for a publishsettings file
        /// </summary>
        public const string DefaultPublishsettingsFilename = "default.publishsettings";

        /// <summary>
        /// The name of the environment variable that contains the connection string for connecting to RDFE. 
        /// The connection string consists of a subscription, certificate credential or certificate reference, and [optional] Management URI
        /// 
        /// Sample: TEST_CONNECTION_STRING=SubscriptionId={subscription-id};ManagementCertificate={cert-thumbprint};BaseUri=https://management.core.windows.net
        /// </summary>
        public const string TestCertificateConnectionStringKey = "TEST_CONNECTION_STRING";

        /// <summary>
        /// The name of an environment variable that provides the path to a publishsettings file that can provide management 
        /// credentials for these tests
        /// 
        /// Sample: TEST_PUBLISHSETTINGS_FILE=C:\foo\bar\mypublishsettings.publishsettings
        /// </summary>
        public const string TestPublishSettingsFileKey = "TEST_PUBLISHSETTINGS_FILE";

        /// <summary>
        /// The name of an environment variable that provides the path to a publishsettings file that can provide management 
        /// credentials for these tests
        /// 
        /// Sample Value 1 - Get token from user and password:
        /// TEST_ORGID_AUTHENTICATION=SubscriptionId={subscription-id};BaseUri={management-uri};UserId={user-id};Password={password}       
        /// 
        /// Sample Value 2 - Prompt for login credentials:
        /// TEST_ORGID_AUTHENTICATION=SubscriptionId={subscription-id};AADAuthEndpoint={authority};BaseUri={management-uri}
        /// </summary>
        public const string TestOrgIdAuthenticationKey = "TEST_ORGID_AUTHENTICATION";


        /// <summary>
        /// Get certificate test credentials and target management URI from environment variables
        /// </summary>
        /// <returns>A test environment containing credentials and target URI, or null if no environment is found</returns>
        protected virtual TestEnvironment GetCertificateTestEnvironment( )
        {
            TestEnvironment environment = null;
            IDictionary<string, string> connections = this.GetTestSettings(TestCertificateConnectionStringKey, ExecutionMode.RDFE);
            string testPublishSettingsString = Environment.GetEnvironmentVariable(TestPublishSettingsFileKey);
            string defaultPublishSettingsFile = Path.Combine(Environment.CurrentDirectory, DefaultPublishsettingsFilename);
            if (File.Exists(defaultPublishSettingsFile))
            {
                TracingAdapter.Information("Getting credentials from local publishsettings file: {0}", defaultPublishSettingsFile);
                environment = GetTestEnvironmentFromPublishSettingsFile(defaultPublishSettingsFile);
            }
            else if (!string.IsNullOrEmpty(testPublishSettingsString))
            {
                TracingAdapter.Information("Getting credentials from publishsettings file in environment variable: {0}={1}", TestPublishSettingsFileKey, testPublishSettingsString);
                environment = GetTestEnvironmentFromPublishSettingsFile(testPublishSettingsString);
            }
            else if (connections != null && connections.Count > 0)
            {
                TracingAdapter.Information("Getting credentials from connection string in environment variable: {0}", TestCertificateConnectionStringKey);
                environment = GetCertificateTestEnvironmentFromConnectionString(connections);
            }

            return environment;
        }

        /// <summary>
        /// Get the test environment from a connection string specifying a certificate
        /// </summary>
        /// <param name="testConnectionString">The connection string to parse</param>
        /// <returns>The test environment from parsing the connection string.</returns>
        protected virtual TestEnvironment GetCertificateTestEnvironmentFromConnectionString(IDictionary<string, string> connections)
        {
            string certificateReference = connections[TestEnvironment.ManagementCertificateKey];
            Debug.Assert(certificateReference != null);
            X509Certificate2 managementCertificate;
            if (IsCertificateReference(certificateReference))
            {
                managementCertificate = GetCertificateFromReference(certificateReference);
            }
            else
            {
                managementCertificate = GetCertificateFromBase64String(certificateReference);
            }

            TestEnvironment currentEnvironment = new TestEnvironment(connections, ExecutionMode.RDFE);
            TestEnvironmentHelpers.GetOrSetMockSubscriptionId(currentEnvironment, connections);
            currentEnvironment.AuthorizationContext = new AuthorizationContext();
            currentEnvironment.AuthorizationContext.Certificate = managementCertificate;

            return currentEnvironment;
        }

        /// <summary>
        /// Get the test environment from a publish settings file
        /// </summary>
        /// <param name="filePath">The full path to the publish settings file</param>
        /// <returns>A Test environment with credentials and BaseUri set from the given PublishSettings file.</returns>
        public static TestEnvironment GetTestEnvironmentFromPublishSettingsFile(string filePath)
        {
            PublishData data = XmlSerializationHelpers.DeserializeXmlFile<PublishData>(filePath);
            string encodedCert = data.Items.First().ManagementCertificate;
            if (string.IsNullOrEmpty(encodedCert))
            {
                encodedCert = data.Items.First().Subscription.First().ManagementCertificate;
            }
            if (!string.IsNullOrEmpty(encodedCert))
            {
                X509Certificate2 managementCert = GetCertificateFromBase64String(encodedCert);
                data.Items.First().ManagementCertificate = managementCert.Thumbprint;
                PublishDataPublishProfileSubscription subscription = data.Items.First().Subscription.First();
                subscription.ServiceManagementUrl = subscription.ServiceManagementUrl ??
                    data.Items[0].Url;
                var environment = new TestEnvironment();
                environment.SubscriptionId = subscription.Id;
                TestEnvironmentHelpers.GetOrSetMockSubscriptionId(environment, null);
                environment.BaseUri = new Uri(subscription.ServiceManagementUrl);
                environment.AuthorizationContext = new AuthorizationContext();
                environment.AuthorizationContext.Certificate = managementCert;
                return environment;
            }

            throw new ArgumentException(string.Format("{0} is not a valid publish settings file, you must provide a valid publish settings " +
                "file in the environment variable {1}", filePath, TestPublishSettingsFileKey));
        }

        /// <summary>
        /// Determine if a given management certificate setting is a base-64 encoded certificate or a thumbprint certificate reference
        /// </summary>
        /// <param name="potentialReference">The certificate reference to check</param>
        /// <returns>True if the given string is a certificate reference, false if it is a base64-encoded certificate</returns>
        static bool IsCertificateReference(string potentialReference)
        {
            return potentialReference.Length < 257;
        }

        /// <summary>
        /// Given a thumbprint string, retrieve the associated certificate
        /// </summary>
        /// <param name="thumbprint">The thumbprint string</param>
        /// <returns>The associated certificate</returns>
        static X509Certificate2 GetCertificateFromReference(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            return collection[0];
        }

        /// <summary>
        /// Create a certificate from a base-64 encoded string
        /// </summary>
        /// <param name="base64String">The encoded string</param>
        /// <returns>The associated certificate</returns>
        static X509Certificate2 GetCertificateFromBase64String(string base64String)
        {
            return new X509Certificate2(Convert.FromBase64String(base64String), string.Empty);
        }

        /// <summary>
        /// Create a Certificate based on current environment variables
        /// </summary>
        /// <returns>The RDFE test environment.</returns>
        protected override TestEnvironment GetTestEnvironmentFromContext()
        {
            TestEnvironment environment = this.GetCertificateTestEnvironment();
            if (environment == null)
            {
                environment = base.LoadTestEnvironment(TestOrgIdAuthenticationKey, ExecutionMode.RDFE);
            }

            return environment;
        }
    }
}
