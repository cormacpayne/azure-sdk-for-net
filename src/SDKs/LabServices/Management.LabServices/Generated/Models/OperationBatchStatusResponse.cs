// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.LabServices.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Status Details of the long running operation for an environment
    /// </summary>
    public partial class OperationBatchStatusResponse
    {
        /// <summary>
        /// Initializes a new instance of the OperationBatchStatusResponse
        /// class.
        /// </summary>
        public OperationBatchStatusResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OperationBatchStatusResponse
        /// class.
        /// </summary>
        /// <param name="items">Gets a collection of items that contain the
        /// operation url and status.</param>
        public OperationBatchStatusResponse(IList<OperationBatchStatusResponseItem> items = default(IList<OperationBatchStatusResponseItem>))
        {
            Items = items;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets a collection of items that contain the operation url and
        /// status.
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public IList<OperationBatchStatusResponseItem> Items { get; private set; }

    }
}
