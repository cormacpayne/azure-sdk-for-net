// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.DataLake.Analytics
{
    using Microsoft.Azure;
    using Microsoft.Azure.Management;
    using Microsoft.Azure.Management.DataLake;
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Microsoft.Rest.Azure.OData;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// DataLakeStoreAccountsOperations operations.
    /// </summary>
    public partial interface IDataLakeStoreAccountsOperations
    {
        /// <summary>
        /// Gets the specified Data Lake Store account details in the specified
        /// Data Lake Analytics account.
        /// </summary>
        /// <param name='resourceGroupName'>
        /// The name of the Azure resource group that contains the Data Lake
        /// Analytics account.
        /// </param>
        /// <param name='accountName'>
        /// The name of the Data Lake Analytics account from which to retrieve
        /// the Data Lake Store account details.
        /// </param>
        /// <param name='dataLakeStoreAccountName'>
        /// The name of the Data Lake Store account to retrieve
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.Azure.CloudException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<DataLakeStoreAccountInfo>> GetWithHttpMessagesAsync(string resourceGroupName, string accountName, string dataLakeStoreAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Updates the Data Lake Analytics account specified to remove the
        /// specified Data Lake Store account.
        /// </summary>
        /// <param name='resourceGroupName'>
        /// The name of the Azure resource group that contains the Data Lake
        /// Analytics account.
        /// </param>
        /// <param name='accountName'>
        /// The name of the Data Lake Analytics account from which to remove
        /// the Data Lake Store account.
        /// </param>
        /// <param name='dataLakeStoreAccountName'>
        /// The name of the Data Lake Store account to remove
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.Azure.CloudException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse> DeleteWithHttpMessagesAsync(string resourceGroupName, string accountName, string dataLakeStoreAccountName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Updates the specified Data Lake Analytics account to include the
        /// additional Data Lake Store account.
        /// </summary>
        /// <param name='resourceGroupName'>
        /// The name of the Azure resource group that contains the Data Lake
        /// Analytics account.
        /// </param>
        /// <param name='accountName'>
        /// The name of the Data Lake Analytics account to which to add the
        /// Data Lake Store account.
        /// </param>
        /// <param name='dataLakeStoreAccountName'>
        /// The name of the Data Lake Store account to add.
        /// </param>
        /// <param name='parameters'>
        /// The details of the Data Lake Store account.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.Azure.CloudException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse> AddWithHttpMessagesAsync(string resourceGroupName, string accountName, string dataLakeStoreAccountName, AddDataLakeStoreParameters parameters = default(AddDataLakeStoreParameters), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets the first page of Data Lake Store accounts linked to the
        /// specified Data Lake Analytics account. The response includes a link
        /// to the next page, if any.
        /// </summary>
        /// <param name='resourceGroupName'>
        /// The name of the Azure resource group that contains the Data Lake
        /// Analytics account.
        /// </param>
        /// <param name='accountName'>
        /// The name of the Data Lake Analytics account for which to list Data
        /// Lake Store accounts.
        /// </param>
        /// <param name='odataQuery'>
        /// OData parameters to apply to the operation.
        /// </param>
        /// <param name='select'>
        /// OData Select statement. Limits the properties on each entry to just
        /// those requested, e.g. Categories?$select=CategoryName,Description.
        /// Optional.
        /// </param>
        /// <param name='count'>
        /// The Boolean value of true or false to request a count of the
        /// matching resources included with the resources in the response,
        /// e.g. Categories?$count=true. Optional.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.Azure.CloudException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<DataLakeStoreAccountInfo>>> ListByAccountWithHttpMessagesAsync(string resourceGroupName, string accountName, ODataQuery<DataLakeStoreAccountInfo> odataQuery = default(ODataQuery<DataLakeStoreAccountInfo>), string select = default(string), bool? count = default(bool?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets the first page of Data Lake Store accounts linked to the
        /// specified Data Lake Analytics account. The response includes a link
        /// to the next page, if any.
        /// </summary>
        /// <param name='nextPageLink'>
        /// The NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.Azure.CloudException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<DataLakeStoreAccountInfo>>> ListByAccountNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
