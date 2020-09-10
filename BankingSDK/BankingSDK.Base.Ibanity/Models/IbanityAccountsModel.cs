using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityAccountsModel
    {
        [JsonProperty("data")]
        public List<AccountDTO> Data { get; set; }
        [JsonProperty("meta")]
        public IbanityMeta Meta { get; set; }
    }

    internal class AccountModel
    {
        [JsonProperty("data")]
        public AccountDTO Data { get; set; }
    }

    internal class AccountDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("attributes")]
        public AccountAttributes Attributes { get; set; }


        [JsonProperty("relationships")]
        public AccountRelationships Relationships { get; set; }
    }

    internal class AccountAttributes
    {
        [JsonProperty("authorizationExpirationExpectedAt")]
        public DateTime AuthorizationExpirationExpectedAt { get; set; }
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        [JsonProperty("availableBalanceChangedAt")]
        public DateTime AvailableBalanceChangedAt { get; set; }
        [JsonProperty("availableBalanceReferenceDate")]
        public DateTime AvailableBalanceReferenceDate { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("currentBalance")]
        public decimal CurrentBalance { get; set; }
        [JsonProperty("currentBalanceChangedAt")]
        public DateTime CurrentBalanceChangedAt { get; set; }
        [JsonProperty("currentBalanceReferenceDate")]
        public DateTime CurrentBalanceReferenceDate { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("reference")]
        public string Reference { get; set; }
        [JsonProperty("referenceType")]
        public string ReferenceType { get; set; }
        [JsonProperty("subtype")]
        public string Subtype { get; set; }
    }

    internal class RelationshipsData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    internal class RelationshipsFinancialInstitution
    {
        [JsonProperty("data")]
        public RelationshipsData Data { get; set; }
    }

    internal class AccountRelationships
    {
        [JsonProperty("financialInstitution")]
        public RelationshipsFinancialInstitution FinancialInstitution { get; set; }
    }
}
