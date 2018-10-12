﻿//
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

using System.Collections.Generic;

namespace Microsoft.Azure.Test.Authentication
{
    public class SpnAuthenticationProvider : AuthenticationProvider
    {
        protected override bool TryDoAuthentication(TestEnvironment env, IDictionary<string, string> settings, ExecutionMode mode, out AuthorizationContext context)
        {
            context = null;
            if (settings == null ||
                !settings.ContainsKey(TestEnvironment.AADApplicationSecretKey) ||
                string.IsNullOrEmpty(env.ServicePrincipal))
            {
                return false;
            }
            
            context = TokenCloudCredentialsHelper.GetApplicationToken(
                env.Endpoints.AADAuthUri.ToString(),
                env.ManagementResource,
                env.GraphResource,
                env.Tenant,
                env.ServicePrincipal,
                settings[TestEnvironment.AADApplicationSecretKey]);

            return true;
        }
    }
}
