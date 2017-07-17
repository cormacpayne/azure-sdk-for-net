// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.DataLake.Analytics.Models
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.DataLake;
    using Microsoft.Azure.Management.DataLake.Analytics;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// A Data Lake Analytics catalog U-SQL database item.
    /// </summary>
    public partial class USqlDatabase : CatalogItem
    {
        /// <summary>
        /// Initializes a new instance of the USqlDatabase class.
        /// </summary>
        public USqlDatabase()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the USqlDatabase class.
        /// </summary>
        /// <param name="computeAccountName">the name of the Data Lake
        /// Analytics account.</param>
        /// <param name="version">the version of the catalog item.</param>
        /// <param name="name">the name of the database.</param>
        public USqlDatabase(string computeAccountName = default(string), System.Guid? version = default(System.Guid?), string name = default(string))
            : base(computeAccountName, version)
        {
            Name = name;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        [JsonProperty(PropertyName = "databaseName")]
        public string Name { get; set; }

    }
}
