using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityPaging
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("after")]
        public string After { get; set; }
        [JsonProperty("before")]
        public string Before { get; set; }
    }
}
