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
    using Rest;
    using Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Describes a Virtual Machine.
    /// </summary>
    [JsonTransformation]
    public partial class VirtualMachine : Resource
    {
        /// <summary>
        /// Initializes a new instance of the VirtualMachine class.
        /// </summary>
        public VirtualMachine() { }

        /// <summary>
        /// Initializes a new instance of the VirtualMachine class.
        /// </summary>
        /// <param name="location">Resource location</param>
        /// <param name="id">Resource Id</param>
        /// <param name="name">Resource name</param>
        /// <param name="type">Resource type</param>
        /// <param name="tags">Resource tags</param>
        /// <param name="plan">The purchase plan when deploying virtual machine
        /// from VM Marketplace images.</param>
        /// <param name="hardwareProfile">The hardware profile.</param>
        /// <param name="storageProfile">The storage profile.</param>
        /// <param name="osProfile">The OS profile.</param>
        /// <param name="networkProfile">The network profile.</param>
        /// <param name="diagnosticsProfile">The diagnostics profile.</param>
        /// <param name="availabilitySet">The reference Id of the availability
        /// set to which the virtual machine belongs.</param>
        /// <param name="provisioningState">The provisioning state, which only
        /// appears in the response.</param>
        /// <param name="instanceView">The virtual machine instance
        /// view.</param>
        /// <param name="licenseType">Specifies that the image or disk that is
        /// being used was licensed on-premises. This element is only used for
        /// images that contain the Windows Server operating system.</param>
        /// <param name="vmId">Specifies the VM unique ID which is a 128-bits
        /// identifier that is encoded and stored in all Azure IaaS VMs SMBIOS
        /// and can be read using platform BIOS commands.</param>
        /// <param name="resources">The virtual machine child extension
        /// resources.</param>
        /// <param name="identity">The identity of the virtual machine, if
        /// configured.</param>
        public VirtualMachine(string location, string id = default(string), string name = default(string), string type = default(string), IDictionary<string, string> tags = default(IDictionary<string, string>), Plan plan = default(Plan), HardwareProfile hardwareProfile = default(HardwareProfile), StorageProfile storageProfile = default(StorageProfile), OSProfile osProfile = default(OSProfile), NetworkProfile networkProfile = default(NetworkProfile), DiagnosticsProfile diagnosticsProfile = default(DiagnosticsProfile), SubResource availabilitySet = default(SubResource), string provisioningState = default(string), VirtualMachineInstanceView instanceView = default(VirtualMachineInstanceView), string licenseType = default(string), string vmId = default(string), IList<VirtualMachineExtension> resources = default(IList<VirtualMachineExtension>), VirtualMachineIdentity identity = default(VirtualMachineIdentity))
            : base(location, id, name, type, tags)
        {
            Plan = plan;
            HardwareProfile = hardwareProfile;
            StorageProfile = storageProfile;
            OsProfile = osProfile;
            NetworkProfile = networkProfile;
            DiagnosticsProfile = diagnosticsProfile;
            AvailabilitySet = availabilitySet;
            ProvisioningState = provisioningState;
            InstanceView = instanceView;
            LicenseType = licenseType;
            VmId = vmId;
            Resources = resources;
            Identity = identity;
        }

        /// <summary>
        /// Gets or sets the purchase plan when deploying virtual machine from
        /// VM Marketplace images.
        /// </summary>
        [JsonProperty(PropertyName = "plan")]
        public Plan Plan { get; set; }

        /// <summary>
        /// Gets or sets the hardware profile.
        /// </summary>
        [JsonProperty(PropertyName = "properties.hardwareProfile")]
        public HardwareProfile HardwareProfile { get; set; }

        /// <summary>
        /// Gets or sets the storage profile.
        /// </summary>
        [JsonProperty(PropertyName = "properties.storageProfile")]
        public StorageProfile StorageProfile { get; set; }

        /// <summary>
        /// Gets or sets the OS profile.
        /// </summary>
        [JsonProperty(PropertyName = "properties.osProfile")]
        public OSProfile OsProfile { get; set; }

        /// <summary>
        /// Gets or sets the network profile.
        /// </summary>
        [JsonProperty(PropertyName = "properties.networkProfile")]
        public NetworkProfile NetworkProfile { get; set; }

        /// <summary>
        /// Gets or sets the diagnostics profile.
        /// </summary>
        [JsonProperty(PropertyName = "properties.diagnosticsProfile")]
        public DiagnosticsProfile DiagnosticsProfile { get; set; }

        /// <summary>
        /// Gets or sets the reference Id of the availability set to which the
        /// virtual machine belongs.
        /// </summary>
        [JsonProperty(PropertyName = "properties.availabilitySet")]
        public SubResource AvailabilitySet { get; set; }

        /// <summary>
        /// Gets the provisioning state, which only appears in the response.
        /// </summary>
        [JsonProperty(PropertyName = "properties.provisioningState")]
        public string ProvisioningState { get; protected set; }

        /// <summary>
        /// Gets the virtual machine instance view.
        /// </summary>
        [JsonProperty(PropertyName = "properties.instanceView")]
        public VirtualMachineInstanceView InstanceView { get; protected set; }

        /// <summary>
        /// Gets or sets specifies that the image or disk that is being used
        /// was licensed on-premises. This element is only used for images that
        /// contain the Windows Server operating system.
        /// </summary>
        [JsonProperty(PropertyName = "properties.licenseType")]
        public string LicenseType { get; set; }

        /// <summary>
        /// Gets specifies the VM unique ID which is a 128-bits identifier that
        /// is encoded and stored in all Azure IaaS VMs SMBIOS and can be read
        /// using platform BIOS commands.
        /// </summary>
        [JsonProperty(PropertyName = "properties.vmId")]
        public string VmId { get; protected set; }

        /// <summary>
        /// Gets the virtual machine child extension resources.
        /// </summary>
        [JsonProperty(PropertyName = "resources")]
        public IList<VirtualMachineExtension> Resources { get; protected set; }

        /// <summary>
        /// Gets or sets the identity of the virtual machine, if configured.
        /// </summary>
        [JsonProperty(PropertyName = "identity")]
        public VirtualMachineIdentity Identity { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public override void Validate()
        {
            base.Validate();
            if (StorageProfile != null)
            {
                StorageProfile.Validate();
            }
            if (Resources != null)
            {
                foreach (var element in Resources)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}

