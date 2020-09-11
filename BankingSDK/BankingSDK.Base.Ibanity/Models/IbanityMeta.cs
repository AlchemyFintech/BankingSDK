using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityMeta
    {
        [JsonProperty("paging")]
        public IbanityPaging Paging { get; set; }
    }
}
