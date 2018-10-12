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
using System.Net.Http;
using System.Reflection;
using Hyak.Common;
using Microsoft.Azure;
using Microsoft.Azure.Test;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Azure.Test.Authentication;

namespace Microsoft.Azure.Test
{
    public partial class TestBase
    {
        /// <summary>
        /// Get a test environment using default options
        /// </summary>
        /// <typeparam name="T">The type of the service client to return</typeparam>
        /// <returns>A Service client using credentials and base uri from the current environment</returns>
        public static T GetServiceClient<T>() where T : class
        {
            return TestBase.GetServiceClient<T>(new RDFETestEnvironmentFactory());
        }

        /// <summary>
        /// Get a test environment, allowing the test to customize the creation options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static T GetServiceClient<T>(TestEnvironmentFactory factory) where T : class
        {
            TestEnvironment currentEnvironment = factory.GetTestEnvironment();
            T client;
            var credentials = new SubscriptionCredentialsAdapter(
                currentEnvironment.AuthorizationContext.TokenCredentials[TokenAudience.Management],
                currentEnvironment.SubscriptionId);

            if (currentEnvironment.UsesCustomUri())
            {
                ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(SubscriptionCloudCredentials), typeof(Uri) });
                client = constructor.Invoke(new object[] { credentials, currentEnvironment.BaseUri }) as T;
            }
            else
            {
                ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(SubscriptionCloudCredentials) });
                client = constructor.Invoke(new object[] { credentials }) as T;
            }

            return AddMockHandler<T>(ref client);
        }

        /// <summary>
        /// Gets a test environment, allowing the test to customize the creation options including the apiversion of the client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public static T GetServiceClient<T>(TestEnvironmentFactory factory, string apiVersion) where T : class
        {
            TestEnvironment currentEnvironment = factory.GetTestEnvironment();
            T client;

            var subscriptionCredentials = new SubscriptionCredentialsAdapter(
                currentEnvironment.AuthorizationContext.TokenCredentials[TokenAudience.Management],
                currentEnvironment.SubscriptionId);

            if (currentEnvironment.UsesCustomUri())
            {
                ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(Uri), typeof(SubscriptionCloudCredentials), typeof(string) });
                client = constructor.Invoke(new object[] {  currentEnvironment.BaseUri, subscriptionCredentials, apiVersion }) as T;
            }
            else
            {
                 ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] {typeof(SubscriptionCloudCredentials), typeof(string) });
                 client = constructor.Invoke(new object[] { subscriptionCredentials, apiVersion }) as T;
            }

            return AddMockHandler<T>(ref client);
        }

        /// <summary>
        /// Gets Graph client for the current test environment 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static T GetGraphServiceClient<T>(
            TestEnvironmentFactory factory, 
            string customTenantId = null) 
            where T : class
        {
            TestEnvironment currentEnvironment = factory.GetTestEnvironment();

            if (!currentEnvironment.AuthorizationContext.TokenCredentials.ContainsKey(TokenAudience.Graph))
            {
                throw new ArgumentNullException(
                    "currentEnvironment.TokenInfo[TokenAudience.Graph]",
                    "Unable to create Graph Management client because Graph authentication token was not acquired during Login.");
            }

            var subscriptionCredentials = new SubscriptionCredentialsAdapter(
                currentEnvironment.AuthorizationContext.TokenCredentials[TokenAudience.Graph],
                currentEnvironment.SubscriptionId);

            T client = null;

            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] 
                    { 
                        typeof(string),
                        typeof(SubscriptionCloudCredentials), 
                        typeof(Uri) 
                    });
            client = constructor.Invoke(new object[] 
                    { 
                        customTenantId ?? currentEnvironment.AuthorizationContext.TenantId,
                        subscriptionCredentials, 
                        currentEnvironment.Endpoints.GraphUri }) as T;

            return AddMockHandler<T>(ref client);
        }

        protected static T AddMockHandler<T>(ref T client) where T : class
        {
            HttpMockServer server;

            try
            {
                server = HttpMockServer.CreateInstance();
            }
            catch (ApplicationException)
            {
                // mock server has never been initialized, we will need to initialize it.
                HttpMockServer.Initialize("TestEnvironment", "InitialCreation");
                server = HttpMockServer.CreateInstance();
            }

            MethodInfo method = typeof(T).GetMethod("WithHandler", new Type[] { typeof(DelegatingHandler) });
            client = method.Invoke(client, new object[] { server }) as T;
            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                PropertyInfo initialTimeout = typeof(T).GetProperty("LongRunningOperationInitialTimeout", typeof(int));
                PropertyInfo retryTimeout = typeof(T).GetProperty("LongRunningOperationRetryTimeout", typeof(int));
                if (initialTimeout != null && retryTimeout != null)
                {
                    initialTimeout.SetValue(client, 0);
                    retryTimeout.SetValue(client, 0);
                }
            }

            return client;
        }
    }
}
