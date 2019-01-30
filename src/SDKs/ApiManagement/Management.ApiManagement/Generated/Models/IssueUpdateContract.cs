// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.ApiManagement.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Issue update Parameters.
    /// </summary>
    [Rest.Serialization.JsonTransformation]
    public partial class IssueUpdateContract
    {
        /// <summary>
        /// Initializes a new instance of the IssueUpdateContract class.
        /// </summary>
        public IssueUpdateContract()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IssueUpdateContract class.
        /// </summary>
        /// <param name="createdDate">Date and time when the issue was
        /// created.</param>
        /// <param name="state">Status of the issue. Possible values include:
        /// 'proposed', 'open', 'removed', 'resolved', 'closed'</param>
        /// <param name="apiId">A resource identifier for the API the issue was
        /// created for.</param>
        /// <param name="title">The issue title.</param>
        /// <param name="description">Text describing the issue.</param>
        /// <param name="userId">A resource identifier for the user created the
        /// issue.</param>
        public IssueUpdateContract(System.DateTime? createdDate = default(System.DateTime?), string state = default(string), string apiId = default(string), string title = default(string), string description = default(string), string userId = default(string))
        {
            CreatedDate = createdDate;
            State = state;
            ApiId = apiId;
            Title = title;
            Description = description;
            UserId = userId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets date and time when the issue was created.
        /// </summary>
        [JsonProperty(PropertyName = "properties.createdDate")]
        public System.DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets status of the issue. Possible values include:
        /// 'proposed', 'open', 'removed', 'resolved', 'closed'
        /// </summary>
        [JsonProperty(PropertyName = "properties.state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets a resource identifier for the API the issue was
        /// created for.
        /// </summary>
        [JsonProperty(PropertyName = "properties.apiId")]
        public string ApiId { get; set; }

        /// <summary>
        /// Gets or sets the issue title.
        /// </summary>
        [JsonProperty(PropertyName = "properties.title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets text describing the issue.
        /// </summary>
        [JsonProperty(PropertyName = "properties.description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a resource identifier for the user created the issue.
        /// </summary>
        [JsonProperty(PropertyName = "properties.userId")]
        public string UserId { get; set; }

    }
}
