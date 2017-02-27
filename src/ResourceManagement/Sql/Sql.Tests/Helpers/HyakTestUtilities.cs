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

using Microsoft.Azure;
using System.Diagnostics;
using System.Net;

namespace Sql.Tests.Helpers
{
    public class HyakTestUtilities
    {
        /// <summary>
        /// Validate the fields of an operation response
        /// </summary>
        /// <param name="opResponse"> The operation response to validate</param>
        /// <param name="expectedStatus">The expected status code </param>
        public static void ValidateOperationResponse(AzureOperationResponse opResponse, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            Debug.Assert(opResponse.StatusCode == expectedStatus);
            Debug.Assert(opResponse.RequestId != null);
        }
    }
}
