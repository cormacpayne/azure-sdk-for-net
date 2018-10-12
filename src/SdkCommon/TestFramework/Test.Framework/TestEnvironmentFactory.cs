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
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Hyak.Common;
using Microsoft.Azure;
using Microsoft.Azure.Test;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Azure.Test.Authentication;
using Microsoft.Azure.Test.Subscriptions;
using Microsoft.Rest;

namespace Microsoft.Azure.Test
{
    public enum ExecutionMode
    {
        CSM,
        RDFE
    };

    public abstract class TestEnvironmentFactory
    {
        /// <summary>
        /// Custom values that should override environment variables during runtime 
        /// </summary>
        public Dictionary<string, string> CustomEnvValues = new Dictionary<string, string>();

        public AuthenticationProvider AuthenticationProvider { get; set; }

        public Func<string, ExecutionMode, IDictionary<string, string>> GetTestSettings { get; set; }

        protected TestEnvironmentFactory()
            : this(null)
        {
        }

        protected TestEnvironmentFactory(params AuthenticationProvider[] providers)
        {
            if (providers == null || providers.Length == 0)
            {
                providers = new AuthenticationProvider[]
                {
                    new RawTokenAuthenticationProvider(),
                    new SpnAuthenticationProvider(),
                    new OrgIdAuthenticationProvider(),
                    new LoginPromptAuthenticationProvider()
                };
            }

            GetTestSettings = (name, mode) =>
                LoadEnvironmentSettings(Environment.GetEnvironmentVariable(name), mode);
            AuthenticationProvider = AuthenticationProvider.CreateChain(providers);
        }

        public virtual TestEnvironment GetTestEnvironment()
        {
            return this.GetTestEnvironmentFromContext();
        }

        protected abstract TestEnvironment GetTestEnvironmentFromContext();

        protected virtual IDictionary<string, string> LoadEnvironmentSettings(string connectionString, ExecutionMode mode)
        {
            IDictionary<string, string> parsedConnection = TestUtilities.ParseConnectionString(connectionString); ;
            if (CustomEnvValues != null && CustomEnvValues.Count > 0)
            {

                foreach (var keyVal in CustomEnvValues)
                {
                    parsedConnection[keyVal.Key] = keyVal.Value;
                }
            }

            return parsedConnection;
        }

        /// <summary>
        /// Return test credentials and URI using AAD auth for an OrgID account.  Use this method with caution, it may take a dependency on ADAL
        /// </summary>
        /// <returns>The test credentials, or null if the appropriate environment variable is not set.</returns>
        protected virtual TestEnvironment LoadTestEnvironment(string envVariableName, ExecutionMode mode)
        {
            IDictionary<string, string> parsedConnection = GetTestSettings(envVariableName, mode);
            TestEnvironment testEnv = new TestEnvironment(parsedConnection, mode);
            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                testEnv.UserName = TestEnvironment.UserIdDefault;
                testEnv.AuthorizationContext = new AuthorizationContext
                {
                    AuthorizationType = AuthorizationType.Token,
                    AccessTokenType = "Bearer"
                };
                testEnv.AuthorizationContext.TokenCredentials[TokenAudience.Management] = new TokenCredentials(TestEnvironment.RawTokenDefault);
                testEnv.AuthorizationContext.TokenCredentials[TokenAudience.Graph] = new TokenCredentials(TestEnvironment.RawTokenDefault);

                testEnv.SubscriptionId = TestEnvironmentHelpers.GetSubscriptionIdFromMocksOrProperties(parsedConnection);
            }
            else //Record or None
            {

                testEnv.AuthorizationContext = this.AuthenticationProvider.Authenticate(testEnv, parsedConnection,
                    mode);
                if (testEnv.CustomAudience == null || testEnv.SubscriptionId == null || 
                    testEnv.Tenant == null || testEnv.UserName == null )
                {
                    TestEnvironmentHelpers.SetSubscriptionIdFromService(testEnv, mode);
                }
                else
                {
                    testEnv.AuthorizationContext.UserId = testEnv.UserName;
                }

                TestEnvironmentHelpers.SetMockSubscriptionId(testEnv.SubscriptionId);
            }

            return testEnv;
        }

    }
}
