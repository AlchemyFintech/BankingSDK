using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityAccountInformationAccessModel
    {
        [JsonProperty("data")]
        public IbanityAccountInformationAccessData Data { get; set; }
    }

    internal class IbanityAccountInformationAccessAttributes
    {
        [JsonProperty("requestedAccountReferences")]
        public List<string> RequestedAccountReferences { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    //internal class IbanityAccounts
    //{
    //    [JsonProperty("links")]
    //    public IbanityRelatedLinks Links { get; set; }
    //}

    //internal class IbanityAccountInformationAccessRelationships
    //{
    //    [JsonProperty("accounts")]
    //    public IbanityAccounts Accounts { get; set; }
    //}

    internal class IbanityAccountInformationAccessData
    {
        [JsonProperty("attributes")]
        public IbanityAccountInformationAccessAttributes Attributes { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("links")]
        public IbanityRedirectLinks Links { get; set; }
        //[JsonProperty("relationships")]
        //public IbanityAccountInformationAccessRelationships Relationships { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
