using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityRedirectLinks
    {
        [JsonProperty("redirect")]
        public string Redirect { get; set; }
    }
}
