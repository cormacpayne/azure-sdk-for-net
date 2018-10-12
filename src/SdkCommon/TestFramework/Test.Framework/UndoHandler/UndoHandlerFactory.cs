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
using System.Reflection;

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// Factory used to discover and construct the available set of OperationUndoHandlers in the AppDomain
    /// as a chain of responsibility for looking up undo operations
    /// </summary>
    public static class UndoHandlerFactory
    {
        /// <summary>
        /// Create a chain of responsibility for determining undo operations using all available OperationUndoHandlers
        /// in the AppDOmain
        /// </summary>
        /// <returns>A chain of responsibility contianing all available undo handlers</returns>
        public static OperationUndoHandler CreateChain()
        {
            return OperationUndoHandler.Create(
                UndoHandlerFactoryAttribute.GetAll().SelectMany<Type, MethodInfo>((t) => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Select<MethodInfo, OperationUndoHandler>(m => m.Invoke(null, null) as OperationUndoHandler)
                .ToArray<OperationUndoHandler>());
        }
    }
}
