using Hyak.Common;
using Microsoft.Azure;
using Microsoft.Azure.Test;
using System;
using System.Net.Http;
using System.Threading;

namespace Spec.TestSupport.Test.Client
{
    public partial class SimpleClient : ServiceClient<SimpleClient>, ISimpleClient
    {
        public SimpleClient()
        {

        }
        
        public SimpleClient(SubscriptionCloudCredentials credentials, Uri baseUri)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }
            this.Credentials = credentials;
            this.BaseUri = baseUri;
            this.Credentials.InitializeServiceClient(this);
        }

        public SimpleClient(SubscriptionCloudCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }
            this.BaseUri = TestEnvironment.EnvEndpoints[EnvironmentNames.Prod].ResourceManagementUri;
            this.Credentials = credentials;
            this.Credentials.InitializeServiceClient(this);
        }

        public string ApiVersion
        {
            get { throw new NotImplementedException(); }
        }

        public Uri BaseUri { get; set;  }

        public SubscriptionCloudCredentials Credentials { get; set; }

        public int LongRunningOperationInitialTimeout  { get; set; }

        public int LongRunningOperationRetryTimeout { get; set; }

        public override SimpleClient WithHandler(System.Net.Http.DelegatingHandler handler)
        {
            return (SimpleClient)WithHandler(new SimpleClient(this.Credentials, this.BaseUri), handler);
        }

        public HttpResponseMessage CsmGetLocation()
        {
            var subscriptionId = this.Credentials.SubscriptionId;
            // Construct URL
            string url = this.BaseUri.AbsoluteUri + "subscriptions/" + this.Credentials.SubscriptionId + "/providers?api-version=2014-04-01-preview";

            // Create HTTP transport objects
            HttpRequestMessage httpRequest = null;

            httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Get;
            httpRequest.RequestUri = new Uri(url);

            // Set Headers
            //httpRequest.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));

            // Set Credentials
            var cancellationToken = new CancellationToken();
            cancellationToken.ThrowIfCancellationRequested();
            this.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            return this.HttpClient.SendAsync(httpRequest, cancellationToken).Result;
        }

        public HttpResponseMessage RdfeCheckSbNamespace()
        {
            var subscriptionId = this.Credentials.SubscriptionId;
            // Construct URL
            string url = this.BaseUri.AbsoluteUri + this.Credentials.SubscriptionId + "/services/ServiceBus/CheckNamespaceAvailability?namespace=somenamespace";

            // Create HTTP transport objects
            HttpRequestMessage httpRequest = null;

            httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Get;
            httpRequest.RequestUri = new Uri(url);

            // Set Headers
            httpRequest.Headers.Add("x-ms-version", "2013-06-01");

            // Set Credentials
            var cancellationToken = new CancellationToken();
            cancellationToken.ThrowIfCancellationRequested();
            this.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            return this.HttpClient.SendAsync(httpRequest, cancellationToken).Result;
        }
    }
}
