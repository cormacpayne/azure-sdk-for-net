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

using System.Collections.Generic;

namespace Microsoft.Azure.Test.Authentication
{
    public abstract class AuthenticationProvider
    {
        protected AuthenticationProvider Next { get; set; }

        public AuthorizationContext Authenticate(TestEnvironment environment,
            IDictionary<string, string> settings, ExecutionMode mode)
        {
            AuthorizationContext context = null;
            if (!TryDoAuthentication(environment, settings, mode, out context) && Next != null)
            {
                context = Next.Authenticate(environment, settings, mode);
            }

            return context;
        }

        protected abstract bool TryDoAuthentication(TestEnvironment env,
            IDictionary<string, string> settings, ExecutionMode mode, out AuthorizationContext context);
        public static AuthenticationProvider CreateChain(params AuthenticationProvider[] providers)
        {
            AuthenticationProvider current = null;
            for (int i = providers.Length-1; i >= 0; --i)
            {
                providers[i].Next = current;
                current = providers[i];
            }

            return current;
        }
    }
}
