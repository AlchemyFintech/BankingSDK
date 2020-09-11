using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityAccessInfo
    {
        [JsonProperty("data")]
        public IbanityAccessData Data { get; set; }
    }

    internal class IbanityAccessData
    {
        [JsonProperty("attributes")]
        public IbanityAccessAttributes Attributes { get; set; }
    }

    internal class IbanityAccessAttributes
    {
        [JsonProperty("token")]
        public string AccessToken { get; set; }
    }
}
