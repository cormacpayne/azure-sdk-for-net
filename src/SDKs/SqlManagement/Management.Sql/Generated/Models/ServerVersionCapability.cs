// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Sql.Models
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.Sql;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The server capabilities.
    /// </summary>
    public partial class ServerVersionCapability
    {
        /// <summary>
        /// Initializes a new instance of the ServerVersionCapability class.
        /// </summary>
        public ServerVersionCapability()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ServerVersionCapability class.
        /// </summary>
        /// <param name="name">The server version name.</param>
        /// <param name="status">The status of the server version. Possible
        /// values include: 'Visible', 'Available', 'Default',
        /// 'Disabled'</param>
        /// <param name="supportedEditions">The list of supported database
        /// editions.</param>
        /// <param name="supportedElasticPoolEditions">The list of supported
        /// elastic pool editions.</param>
        public ServerVersionCapability(string name = default(string), CapabilityStatus? status = default(CapabilityStatus?), IList<EditionCapability> supportedEditions = default(IList<EditionCapability>), IList<ElasticPoolEditionCapability> supportedElasticPoolEditions = default(IList<ElasticPoolEditionCapability>))
        {
            Name = name;
            Status = status;
            SupportedEditions = supportedEditions;
            SupportedElasticPoolEditions = supportedElasticPoolEditions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets the server version name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the status of the server version. Possible values include:
        /// 'Visible', 'Available', 'Default', 'Disabled'
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public CapabilityStatus? Status { get; private set; }

        /// <summary>
        /// Gets the list of supported database editions.
        /// </summary>
        [JsonProperty(PropertyName = "supportedEditions")]
        public IList<EditionCapability> SupportedEditions { get; private set; }

        /// <summary>
        /// Gets the list of supported elastic pool editions.
        /// </summary>
        [JsonProperty(PropertyName = "supportedElasticPoolEditions")]
        public IList<ElasticPoolEditionCapability> SupportedElasticPoolEditions { get; private set; }

    }
}
