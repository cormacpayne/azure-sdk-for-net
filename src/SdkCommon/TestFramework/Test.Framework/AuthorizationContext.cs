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

using Microsoft.Azure.Test.Authentication;
using Microsoft.Rest;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Azure.Test
{
    public enum AuthorizationType
    {
        Token,
        Certificate
    }

    public enum TokenAudience
    {
        Management,
        Graph
    }

    public class AuthorizationContext
    {
        public AuthorizationContext()
        {
            TokenCredentials = new Dictionary<TokenAudience, TokenCredentials>();
        }

        public AuthorizationType AuthorizationType { get; internal set; }

        public IDictionary<TokenAudience, TokenCredentials> TokenCredentials { get; internal set; }

        public string AccessTokenType { get; internal set; }

        public string TenantId { get; internal set; }

        public string UserId { get; internal set; }

        public string UserDomain { get; internal set; }

        public X509Certificate2 Certificate { get; internal set; }

        public SubscriptionCloudCredentials GetSubscriptionCredentials(string subscriptionId, TokenAudience audience = TokenAudience.Management)
        {
            switch (AuthorizationType)
            {
                case AuthorizationType.Certificate:
                    return new CertificateCloudCredentials(subscriptionId, Certificate);
                    break;
                case AuthorizationType.Token:
                default:
                    return new SubscriptionCredentialsAdapter(TokenCredentials[audience], subscriptionId);
                    break;
            }
        }

    }
}
