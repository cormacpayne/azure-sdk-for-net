// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Sql.Models
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.Sql;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An Azure SQL Database server.
    /// </summary>
    [Rest.Serialization.JsonTransformation]
    public partial class Server : TrackedResource
    {
        /// <summary>
        /// Initializes a new instance of the Server class.
        /// </summary>
        public Server()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Server class.
        /// </summary>
        /// <param name="location">Resource location.</param>
        /// <param name="id">Resource ID.</param>
        /// <param name="name">Resource name.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="tags">Resource tags.</param>
        /// <param name="identity">The Azure Active Directory identity of the
        /// server.</param>
        /// <param name="kind">Kind of sql server. This is metadata used for
        /// the Azure portal experience.</param>
        /// <param name="administratorLogin">Administrator username for the
        /// server. Once created it cannot be changed.</param>
        /// <param name="administratorLoginPassword">The administrator login
        /// password (required for server creation).</param>
        /// <param name="version">The version of the server.</param>
        /// <param name="state">The state of the server.</param>
        /// <param name="fullyQualifiedDomainName">The fully qualified domain
        /// name of the server.</param>
        public Server(string location, string id = default(string), string name = default(string), string type = default(string), IDictionary<string, string> tags = default(IDictionary<string, string>), ResourceIdentity identity = default(ResourceIdentity), string kind = default(string), string administratorLogin = default(string), string administratorLoginPassword = default(string), string version = default(string), string state = default(string), string fullyQualifiedDomainName = default(string))
            : base(location, id, name, type, tags)
        {
            Identity = identity;
            Kind = kind;
            AdministratorLogin = administratorLogin;
            AdministratorLoginPassword = administratorLoginPassword;
            Version = version;
            State = state;
            FullyQualifiedDomainName = fullyQualifiedDomainName;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the Azure Active Directory identity of the server.
        /// </summary>
        [JsonProperty(PropertyName = "identity")]
        public ResourceIdentity Identity { get; set; }

        /// <summary>
        /// Gets kind of sql server. This is metadata used for the Azure portal
        /// experience.
        /// </summary>
        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; private set; }

        /// <summary>
        /// Gets or sets administrator username for the server. Once created it
        /// cannot be changed.
        /// </summary>
        [JsonProperty(PropertyName = "properties.administratorLogin")]
        public string AdministratorLogin { get; set; }

        /// <summary>
        /// Gets or sets the administrator login password (required for server
        /// creation).
        /// </summary>
        [JsonProperty(PropertyName = "properties.administratorLoginPassword")]
        public string AdministratorLoginPassword { get; set; }

        /// <summary>
        /// Gets or sets the version of the server.
        /// </summary>
        [JsonProperty(PropertyName = "properties.version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets the state of the server.
        /// </summary>
        [JsonProperty(PropertyName = "properties.state")]
        public string State { get; private set; }

        /// <summary>
        /// Gets the fully qualified domain name of the server.
        /// </summary>
        [JsonProperty(PropertyName = "properties.fullyQualifiedDomainName")]
        public string FullyQualifiedDomainName { get; private set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public override void Validate()
        {
            base.Validate();
        }
    }
}
