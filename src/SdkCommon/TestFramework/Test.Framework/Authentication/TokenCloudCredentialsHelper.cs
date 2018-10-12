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
using System.Linq;
using System.Threading;
using Microsoft.Azure.Test;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Rest;

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// Helper for TokenCloudCredentials
    /// </summary>
    public class TokenCloudCredentialsHelper
    {
        /// <summary>
        /// Returns token (requires user input)
        /// </summary>
        /// <returns></returns>
        public static AuthorizationContext GetUserToken(
            string authEndpoint,
            string tenant,
            string clientId,
            string managementResource,
            string graphResource,
            string userName,
            string password)
        {
            TokenCredentials mgmAuthResult = null;
            TokenCredentials graphAuthResult = null;

            var mgmSettings = new ActiveDirectoryServiceSettings()
            {
                AuthenticationEndpoint = new Uri(authEndpoint),
                TokenAudience = new Uri(managementResource)
            };
            var grpSettings = new ActiveDirectoryServiceSettings()
            {
                AuthenticationEndpoint = new Uri(authEndpoint),
                TokenAudience = new Uri(graphResource)
            };

            if (userName != null && password != null)
            {
#if FullNetFx
                mgmAuthResult = (TokenCredentials) UserTokenProvider
                                .LoginSilentAsync(clientId, tenant, userName, password, mgmSettings)
                                .ConfigureAwait(false).GetAwaiter().GetResult();

                try
                {
                    graphAuthResult = (TokenCredentials)UserTokenProvider
                                    .LoginSilentAsync(clientId, tenant, userName, password, grpSettings)
                                    .ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch
                {
                    // not all the accounts have Graph endpoint access
                }
#else
                throw new NotSupportedException("Username/Password login is supported only in NET452 and above projects");
#endif
            }
            else
            {
#if FullNetFx
                var clientSettings = new ActiveDirectoryClientSettings()
                {
                    ClientId = clientId,
                    ClientRedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob"),
                    PromptBehavior = PromptBehavior.Auto
                };
                mgmAuthResult = (TokenCredentials)UserTokenProvider
                              .LoginWithPromptAsync(tenant, clientSettings, mgmSettings)
                              .ConfigureAwait(false).GetAwaiter().GetResult();
                try
                {
                    graphAuthResult = (TokenCredentials)UserTokenProvider
                                   .LoginWithPromptAsync(tenant, clientSettings, grpSettings)
                                   .ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch
                {
                    // not all the accounts have Graph endpoint access
                }
#else
                throw new NotSupportedException("Interactive Login is supported only in NET452 and above projects");
#endif
            }

            var retValue = new AuthorizationContext();

            if (mgmAuthResult == null || graphAuthResult == null)
            {
                throw new ApplicationException("Error during authentication.");
            }

            retValue.TokenCredentials[TokenAudience.Management] = mgmAuthResult;
            if (graphAuthResult != null)
            {
                retValue.TokenCredentials[TokenAudience.Graph] = graphAuthResult;
            }
            retValue.TenantId = mgmAuthResult.TenantId;

            retValue.UserId = mgmAuthResult.CallerId;
            retValue.UserDomain = retValue.UserId
                .Split(new[] {"@"}, StringSplitOptions.RemoveEmptyEntries)
                .Last();

            return retValue;
        }

        public static AuthorizationContext GetApplicationToken(
            string authEndpoint,
            string managementResource,
            string graphResource,
            string tenant,
            string clientId,
            string secret)
        {
            TokenCredentials mgmAuthResult = null;
            TokenCredentials graphAuthResult = null;

            var mgmSettings = new ActiveDirectoryServiceSettings()
            {
                AuthenticationEndpoint = new Uri(TestEnvironmentHelpers.EnsureTrailingSlash(authEndpoint) + tenant),
                TokenAudience = new Uri(managementResource)
            };
            var grpSettings = new ActiveDirectoryServiceSettings()
            {
                AuthenticationEndpoint = new Uri(authEndpoint),
                TokenAudience = new Uri(graphResource)
            };

            mgmAuthResult = (TokenCredentials)ApplicationTokenProvider
                            .LoginSilentAsync(tenant, clientId, secret, mgmSettings)
                            .ConfigureAwait(false).GetAwaiter().GetResult();

            if (mgmAuthResult == null)
            {
                throw new ApplicationException("Error during SPN authentication.");
            }

            var retValue = new AuthorizationContext();

            try
            {
                graphAuthResult = (TokenCredentials)ApplicationTokenProvider
                               .LoginSilentAsync(tenant, clientId, secret, grpSettings)
                               .ConfigureAwait(false).GetAwaiter().GetResult();

                retValue.TokenCredentials[TokenAudience.Graph] = graphAuthResult;
            }
            catch
            {
                // not all the accounts have Graph endpoint access
            }

            retValue.TokenCredentials[TokenAudience.Management] = mgmAuthResult;
            retValue.TenantId = mgmAuthResult.TenantId;

            return retValue;
        }
    }
}
