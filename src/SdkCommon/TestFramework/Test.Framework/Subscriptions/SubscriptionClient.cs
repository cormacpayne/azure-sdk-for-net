using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Threading;

namespace Microsoft.Azure.Test.Subscriptions
{
    class SubscriptionClient
    {
        public static List<SubscriptionInfo> ListSubscriptions(ExecutionMode mode, string baseuri, AuthorizationContext context)
        {
            if (mode == ExecutionMode.CSM)
            {
                return ListCSMSubscriptions(baseuri, context);
            }
            else
            {
                return listRDFESubscriptions(baseuri, context);
            }
        }

        private static List<SubscriptionInfo> ListCSMSubscriptions(string baseuri, AuthorizationContext context)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Format("{0}/subscriptions?api-version=2014-04-01-preview", baseuri))
            };

            context.TokenCredentials[TokenAudience.Management]
                .ProcessHttpRequestAsync(request, CancellationToken.None)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            string jsonString = response.Content.ReadAsStringAsync().Result;

            var jsonResult = JObject.Parse(jsonString);
            var results = ((JArray)jsonResult["value"]).Select(item => new SubscriptionInfo((JObject)item)).ToList();
            return results;
        }

        private static List<SubscriptionInfo> listRDFESubscriptions(string baseuri, AuthorizationContext context)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Format("{0}/subscriptions", baseuri))
            };

            context.TokenCredentials[TokenAudience.Management]
                .ProcessHttpRequestAsync(request, CancellationToken.None)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            request.Headers.Add("x-ms-version", "2013-08-01");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            string xmlString = response.Content.ReadAsStringAsync().Result;

            XDocument doc = XDocument.Parse(xmlString);
            XNamespace ns = "http://schemas.microsoft.com/windowsazure";
            return doc.Element(ns + "Subscriptions")
                .Elements(ns + "Subscription")
                .Select(el => new SubscriptionInfo
                {
                    SubscriptionId = (string)(el.Element(ns + "SubscriptionID")),
                    DisplayName = (string)(el.Element(ns + "SubscriptionName")),
                    State = (string)(el.Element(ns + "SubscriptionStatus"))
                }).ToList();
        }
    }
}
