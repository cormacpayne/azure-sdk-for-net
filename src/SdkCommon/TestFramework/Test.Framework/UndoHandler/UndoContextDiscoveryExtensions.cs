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

using Microsoft.WindowsAzure.Management.ResourceManagement.Testing;
using Microsoft.Azure.Test;

namespace Microsoft.WindowsAzure.Management.WebSites.Testing
{
    /// <summary>
    /// Class used to discover and construct undo handlers available in the app domain
    /// </summary>
    /// 
    [UndoHandlerFactory]
    public static partial class UndoContextDiscoveryExtensions
    {
        /// <summary>
        /// Create an undo handler for WebSite operations
        /// </summary>
        /// <returns>An undo handler for WebSite  operations</returns>
        public static OperationUndoHandler CreateWebSitesUndoHandler()
        {
            return new WebSiteUndoHandler();
        }

        /// <summary>
        /// Create an undo handler for ResourceManager operations
        /// </summary>
        /// <returns>An undo handler for ResourceManager operations</returns>
        public static OperationUndoHandler CreateResourceManagerUndoHandler()
        {
            return new ResourceManagementUndoHandler();
        }
    }
}

