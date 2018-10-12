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

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// A simple pass through undo handler, this is always the head of the undo handler chain - used to abstract away the
    /// undo handler implementation from users
    /// </summary>
    public class DelegatingOperationUndoHandler : OperationUndoHandler
    {
        /// <summary>
        /// Placeholder required to implement aconcrewte class
        /// </summary>
        /// <param name="client">The implementing class for operation and undo operation</param>
        /// <param name="method">The name of the method</param>
        /// <param name="parameters">The method parameters</param>
        /// <param name="undoFunction">The undo action for the given method, if any exists</param>
        /// <returns>false</returns>
        protected override bool TryFindUndoFunction(object client, string method, IDictionary<string, object> parameters, out Action undoFunction)
        {
            undoFunction = null;
            return false;
        }
    }

}
