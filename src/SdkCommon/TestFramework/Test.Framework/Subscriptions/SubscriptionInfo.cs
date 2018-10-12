using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Test.Subscriptions
{
  class SubscriptionInfo
  {
    public SubscriptionInfo()
    {

    }

    public SubscriptionInfo(JObject resultObject)
    {
        Id = (string)(resultObject["id"]);
        SubscriptionId = (string)(resultObject["subscriptionId"]);
        DisplayName = (string)(resultObject["displayName"]);
        State = (string)(resultObject["state"]);
    }

    public string Id { get; set; }
    public string SubscriptionId { get; set; }
    public string DisplayName { get; set; }
    public string State { get; set; }
  }
}
