// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.DeviceProvisioningServices.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Error response containing message and code.
    /// </summary>
    public partial class ErrorMesssage
    {
        /// <summary>
        /// Initializes a new instance of the ErrorMesssage class.
        /// </summary>
        public ErrorMesssage()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ErrorMesssage class.
        /// </summary>
        /// <param name="code">standard error code</param>
        /// <param name="message">standard error description</param>
        /// <param name="details">detailed summary of error</param>
        public ErrorMesssage(string code = default(string), string message = default(string), string details = default(string))
        {
            Code = code;
            Message = message;
            Details = details;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets standard error code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets standard error description
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets detailed summary of error
        /// </summary>
        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }

    }
}