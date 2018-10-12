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
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Azure.Test.Subscriptions;

namespace Microsoft.Azure.Test
{
    public static class TestEnvironmentHelpers
    {
        /// <summary>
        /// Returns the input string with a trailign slash - if the input string ends with a slash, it
        /// will simply be mirrored.
        /// </summary>
        /// <param name="uri">The string uri to check.</param>
        /// <returns>The input uri with a trailing slash.</returns>
        public static string EnsureTrailingSlash(string uri)
        {
            if (uri.EndsWith("/"))
            {
                return uri;
            }

            return string.Format("{0}/", uri);
        }

        /// <summary>
        /// Verifies that the gievn endpoint matches the expected manageemnt uri for a given mode.
        /// </summary>
        /// <param name="testEndpoint">The endpoints to check with</param>
        /// <param name="mode">The mode (RDFE or CSM)</param>
        /// <param name="endpointValue">The string value to check agaisnt the exected value.</param>
        /// <returns>True if the values match, otherwise false.</returns>
        public static bool MatchEnvironmentBaseUri(TestEndpoints testEndpoint, ExecutionMode mode, string endpointValue)
        {
            endpointValue = EnsureTrailingSlash(endpointValue);
            if (mode == ExecutionMode.RDFE)
            {
                return string.Equals(testEndpoint.ServiceManagementUri.ToString(), endpointValue, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(testEndpoint.ResourceManagementUri.ToString(), endpointValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determine whether the given connection string contains any custom endpoints
        /// </summary>
        /// <param name="connection">The connections tring to check.</param>
        /// <returns>True if the connection string containbs at least one endpoint.  Otherwise false.</returns>
        public static bool ConnectionStringContainsEndpoint(IDictionary<string, string> connection)
        {
            return new[]
            {
                ConnectionStringFields.BaseUri,
                ConnectionStringFields.GraphUri,
                ConnectionStringFields.GalleryUri,
                ConnectionStringFields.AADAuthenticationEndpoint,
                ConnectionStringFields.IbizaPortalUri,
                ConnectionStringFields.RdfePortalUri,
                ConnectionStringFields.DataLakeStoreServiceUri,
                ConnectionStringFields.DataLakeAnalyticsCatalogAndJobServiceUri
            }.Any(connection.ContainsKey);
        }

        /// <summary>
        /// When running in playback mode, get the subscription id from mocks if available,
        /// or from connection settings, if not.  Throws if no subscription id is available.
        /// </summary>
        /// <param name="testEnv">The test environment </param>
        /// <param name="parsedConnection"></param>
        public static string GetSubscriptionIdFromMocksOrProperties(
        IDictionary<string, string> parsedConnection)
        {
            string subscriptionId = null;
            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                if (HttpMockServer.Variables.ContainsKey(TestEnvironment.SubscriptionIdKey))
                {
                    subscriptionId = HttpMockServer.Variables[TestEnvironment.SubscriptionIdKey];
                }
                else if (parsedConnection != null)
                {
                    if (!parsedConnection.ContainsKey(TestEnvironment.SubscriptionIdKey))
                    {
                        throw new Exception(
                            "Subscription ID is not present in the recorded mock or in environment variables.");
                    }

                    subscriptionId = parsedConnection[TestEnvironment.SubscriptionIdKey];
                }
                else
                {
                    throw new Exception(
                        "Subscription ID is not present in the recorded mock or in environment variables.");
                }
            }

            return subscriptionId;
        }

        /// <summary>
        /// Set the subscription id in http recorder from the given test environment
        /// </summary>
        /// <param name="subscriptionId">The subscription id to set for mocks</param>
        public static void SetMockSubscriptionId(string subscriptionId)
        {
            if (HttpMockServer.Mode != HttpRecorderMode.Playback) //Record or None
            {
                // Preserve/restore subscription ID
                HttpMockServer.Variables[TestEnvironment.SubscriptionIdKey] = subscriptionId;
            }
        }

        /// <summary>
        /// Get the subscriptionId from mocks if in playback mode, otherwise set the subscription id from
        /// mocks in the given test environment.
        /// </summary>
        /// <param name="testEnv">The current test environment</param>
        /// <param name="connectionString">The connection string containign test environment settings</param>
        public static void GetOrSetMockSubscriptionId(TestEnvironment testEnv, IDictionary<string, string> parsedConnection)
        {
            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                testEnv.SubscriptionId = GetSubscriptionIdFromMocksOrProperties(parsedConnection);
            }
            else if (testEnv != null)
            {
                SetMockSubscriptionId(testEnv.SubscriptionId);
            }
        }

        /// <summary>
        /// Set the subscription id for the given test environment using the service, using the other 
        /// settings from the test environment - if the test environment contains a subscription id, 
        /// this will verify that the given credentials have access to that subscription.  If there 
        /// is no specified subscription id, set the test environment subscription id if the 
        /// given credentials have access to only one subscription.
        /// </summary>
        /// <param name="testEnv">The test environemtn</param>
        /// <param name="mode"></param>
        public static void SetSubscriptionIdFromService(TestEnvironment testEnv, ExecutionMode mode)
        {
            if (testEnv == null || testEnv.BaseUri == null || testEnv.AuthorizationContext == null ||
                testEnv.AuthorizationContext == null)
            {
                throw new InvalidOperationException(
                    "There was an error in setting up your test environment from settings.  Ensure that your connection strings are set appropriately.");
            }

            //Getting subscriptions from server
            var subscriptions = SubscriptionClient.ListSubscriptions(
                mode,
                testEnv.BaseUri.ToString(),
                testEnv.AuthorizationContext);

            if (subscriptions.Count == 0)
            {
                throw new Exception("Logged in account had no associated subscriptions. We are in " +
                                    testEnv.Endpoints.Name + " environment and the tenant is " +
                                    testEnv.Tenant + ". Please check if the subscription is in the " +
                                    "correct tenant and environment. You can set the envt. variable - " +
                                    "TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId=<subscription-id>;" +
                                    "Environment=<Env-name>;Tenant=<tenant-id>");
            }

            //SubscriptionId is provided in envt. variable
            if (testEnv.SubscriptionId != null)
            {
                if (!subscriptions.Any(item => item.SubscriptionId == testEnv.SubscriptionId))
                {
                    throw new Exception("The provided SubscriptionId in the envt. variable - \"" + testEnv.SubscriptionId +
                                        "\" does not match the list of subscriptions associated with this account.");
                }
            }
            else
            {
                if (subscriptions.Count > 1)
                {
                    throw new Exception("There are multiple subscriptions associated with the logged in account. " +
                                        "Please specify the subscription to use in the connection string. Please set " +
                                        "the envt. variable - TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId=<subscription-id>");
                }

                testEnv.SubscriptionId = subscriptions[0].SubscriptionId;
            }

            if (testEnv.SubscriptionId == null)
            {
                throw new Exception("Subscription Id was not provided in environment variable. " + "Please set " +
                                    "the envt. variable - TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId=<subscription-id>");
            }

            testEnv.UserName = testEnv.AuthorizationContext.UserId;
        }

    }
}
