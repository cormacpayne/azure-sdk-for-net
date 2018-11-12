// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.DevTestLabs.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Properties of a daily schedule.
    /// </summary>
    public partial class DayDetailsFragment
    {
        /// <summary>
        /// Initializes a new instance of the DayDetailsFragment class.
        /// </summary>
        public DayDetailsFragment()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DayDetailsFragment class.
        /// </summary>
        /// <param name="time">The time of day the schedule will occur.</param>
        public DayDetailsFragment(string time = default(string))
        {
            Time = time;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the time of day the schedule will occur.
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

    }
}
