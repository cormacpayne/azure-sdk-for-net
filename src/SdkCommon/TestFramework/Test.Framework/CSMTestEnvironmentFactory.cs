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
    public class CSMTestEnvironmentFactory : TestEnvironmentFactory
    {
        public CSMTestEnvironmentFactory() : base()
        {
            
        }
        /// <summary>
        /// The environment variable name for CSM OrgId authentication
        /// 
        /// Sample Value 1 - Get token from user and password:
        /// TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId={subscription-id};BaseUri=https://api-next.resources.windows-int.net/;UserId={user-id};Password={password}       
        /// 
        /// Sample Value 2 - Prompt for login credentials:
        /// TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId={subscription-id};AADAuthEndpoint=https://login.windows-ppe.net/;BaseUri=https://api-next.resources.windows-int.net/
        /// </summary>
        const string TestCSMOrgIdConnectionStringKey = "TEST_CSM_ORGID_AUTHENTICATION";

        protected override TestEnvironment GetTestEnvironmentFromContext()
        {
            return base.LoadTestEnvironment(TestCSMOrgIdConnectionStringKey, ExecutionMode.CSM);
        }
    }
}
