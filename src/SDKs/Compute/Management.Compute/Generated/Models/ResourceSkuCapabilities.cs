// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Compute.Models
{
    using Azure;
    using Management;
    using Compute;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Describes The SKU capabilites object.
    /// </summary>
    public partial class ResourceSkuCapabilities
    {
        /// <summary>
        /// Initializes a new instance of the ResourceSkuCapabilities class.
        /// </summary>
        public ResourceSkuCapabilities() { }

        /// <summary>
        /// Initializes a new instance of the ResourceSkuCapabilities class.
        /// </summary>
        /// <param name="name">An invariant to describe the feature.</param>
        /// <param name="value">An invariant if the feature is measured by
        /// quantity.</param>
        public ResourceSkuCapabilities(string name = default(string), string value = default(string))
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets an invariant to describe the feature.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets an invariant if the feature is measured by quantity.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public string Value { get; protected set; }

    }
}

