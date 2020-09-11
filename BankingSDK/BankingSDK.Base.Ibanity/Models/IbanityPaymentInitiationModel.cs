using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityPaymentInitiationModel
    {
        [JsonProperty("data")]
        public PaymentInitiationData Data { get; set; }
    }

    internal class PaymentInitiationAttributes
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("consentReference")]
        public string ConsentReference { get; set; }
        [JsonProperty("creditorAccountReference")]
        public string CreditorAccountReference { get; set; }
        [JsonProperty("creditorAccountReferenceType")]
        public string CreditorAccountReferenceType { get; set; }
        [JsonProperty("creditorAgent")]
        public string CreditorAgent { get; set; }
        [JsonProperty("creditorAgentType")]
        public string CreditorAgentType { get; set; }
        [JsonProperty("creditorName")]
        public string CreditorName { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("debtorAccountReference")]
        public string DebtorAccountReference { get; set; }
        [JsonProperty("debtorAccountReferenceType")]
        public string DebtorAccountReferenceType { get; set; }
        [JsonProperty("debtorName")]
        public string DebtorName { get; set; }
        [JsonProperty("endToEndId")]
        public string EndToEndId { get; set; }
        [JsonProperty("productType")]
        public string ProductType { get; set; }
        [JsonProperty("remittanceInformation")]
        public string RemittanceInformation { get; set; }
        [JsonProperty("remittanceInformationType")]
        public string RemittanceInformationType { get; set; }
        [JsonProperty("requestedExecutionDate")]
        public string RequestedExecutionDate { get; set; }
        [JsonProperty("status")]
        public object Status { get; set; }
    }

    internal class PaymentInitiationFinancialInstitutionData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    internal class PaymentInitiationFinancialInstitution
    {
        [JsonProperty("data")]
        public PaymentInitiationFinancialInstitutionData Data { get; set; }
        [JsonProperty("links")]
        public IbanityRedirectLinks Links { get; set; }
    }

    internal class PaymentInitiationRelationships
    {
        [JsonProperty("financialInstitution")]
        public PaymentInitiationFinancialInstitution FinancialInstitution { get; set; }
    }

    internal class PaymentInitiationData
    {
        [JsonProperty("attributes")]
        public PaymentInitiationAttributes Attributes { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("links")]
        public IbanityRedirectLinks Links { get; set; }
        [JsonProperty("relationships")]
        public PaymentInitiationRelationships Relationships { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
