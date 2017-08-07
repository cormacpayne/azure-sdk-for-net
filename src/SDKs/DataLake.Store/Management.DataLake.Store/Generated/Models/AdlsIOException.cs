// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.DataLake.Store.Models
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.DataLake;
    using Microsoft.Azure.Management.DataLake.Store;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// A WebHDFS exception thrown indicating there was an IO (read or write)
    /// error. Thrown when a 403 error response code is returned (forbidden).
    /// </summary>
    [Newtonsoft.Json.JsonObject("IOException")]
    public partial class AdlsIOException : AdlsRemoteException
    {
        /// <summary>
        /// Initializes a new instance of the AdlsIOException class.
        /// </summary>
        public AdlsIOException()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AdlsIOException class.
        /// </summary>
        /// <param name="javaClassName">the full class package name for the
        /// exception thrown, such as
        /// 'java.lang.IllegalArgumentException'.</param>
        /// <param name="message">the message associated with the exception
        /// that was thrown, such as 'Invalid value for webhdfs parameter
        /// "permission":...'.</param>
        public AdlsIOException(string javaClassName = default(string), string message = default(string))
            : base(javaClassName, message)
        {
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

    }
}
