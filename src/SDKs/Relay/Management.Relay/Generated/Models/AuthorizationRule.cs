// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Relay.Models
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.Relay;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Description of a Namespace AuthorizationRules.
    /// </summary>
    [Rest.Serialization.JsonTransformation]
    public partial class AuthorizationRule : Resource
    {
        /// <summary>
        /// Initializes a new instance of the AuthorizationRule class.
        /// </summary>
        public AuthorizationRule()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AuthorizationRule class.
        /// </summary>
        /// <param name="id">Resource Id</param>
        /// <param name="name">Resource name</param>
        /// <param name="type">Resource type</param>
        /// <param name="rights">The rights associated with the rule.</param>
        public AuthorizationRule(string id = default(string), string name = default(string), string type = default(string), IList<AccessRights?> rights = default(IList<AccessRights?>))
            : base(id, name, type)
        {
            Rights = rights;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the rights associated with the rule.
        /// </summary>
        [JsonProperty(PropertyName = "properties.rights")]
        public IList<AccessRights?> Rights { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Rights != null)
            {
                if (Rights.Count != Rights.Distinct().Count())
                {
                    throw new ValidationException(ValidationRules.UniqueItems, "Rights");
                }
            }
        }
    }
}
