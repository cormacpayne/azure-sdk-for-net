// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Batch.Protocol.Models
{
    using System.Linq;

    /// <summary>
    /// Additional parameters for the ListNext operation.
    /// </summary>
    public partial class PoolListNextOptions
    {
        /// <summary>
        /// Initializes a new instance of the PoolListNextOptions class.
        /// </summary>
        public PoolListNextOptions() { }

        /// <summary>
        /// Initializes a new instance of the PoolListNextOptions class.
        /// </summary>
        /// <param name="clientRequestId">The caller-generated request
        /// identity, in the form of a GUID with no decoration such as curly
        /// braces, e.g. 9C4D50EE-2D56-4CD3-8152-34347DC9F2B0.</param>
        /// <param name="returnClientRequestId">Whether the server should
        /// return the client-request-id in the response.</param>
        /// <param name="ocpDate">The time the request was issued. Client
        /// libraries typically set this to the current system clock time; set
        /// it explicitly if you are calling the REST API directly.</param>
        public PoolListNextOptions(System.Guid? clientRequestId = default(System.Guid?), bool? returnClientRequestId = default(bool?), System.DateTime? ocpDate = default(System.DateTime?))
        {
            this.ClientRequestId = clientRequestId;
            this.ReturnClientRequestId = returnClientRequestId;
            this.OcpDate = ocpDate;
        }

        /// <summary>
        /// Gets or sets the caller-generated request identity, in the form of
        /// a GUID with no decoration such as curly braces, e.g.
        /// 9C4D50EE-2D56-4CD3-8152-34347DC9F2B0.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "")]
        public System.Guid? ClientRequestId { get; set; }

        /// <summary>
        /// Gets or sets whether the server should return the client-request-id
        /// in the response.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "")]
        public bool? ReturnClientRequestId { get; set; }

        /// <summary>
        /// Gets or sets the time the request was issued. Client libraries
        /// typically set this to the current system clock time; set it
        /// explicitly if you are calling the REST API directly.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Microsoft.Rest.Serialization.DateTimeRfc1123JsonConverter))]
        [Newtonsoft.Json.JsonProperty(PropertyName = "")]
        public System.DateTime? OcpDate { get; set; }

    }
}
