using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models.Requests
{
    internal class IbanityPaymentInitiationRequest
    {
        public IbanityPaymentInitiationData Data { get; set; }
    }
    internal class IbanityPaymentInitiationAttributes
    {
        public string RedirectUri { get; set; }
        public string ConsentReference { get; set; }
        public string ProductType { get; set; }
        public string RemittanceInformation { get; set; }
        public string RemittanceInformationType { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime RequestedExecutionDate { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string DebtorName { get; set; }
        public string DebtorAccountReference { get; set; }
        public string DebtorAccountReferenceType { get; set; }
        public string CreditorName { get; set; }
        public string CreditorAccountReference { get; set; }
        public string CreditorAccountReferenceType { get; set; }
        public string CreditorAgent { get; set; }
        public string CreditorAgentType { get; set; }
        public string EndToEndId { get; set; }
        public string Locale { get; set; }
        public string CustomerIpAddress { get; set; }
    }

    internal class IbanityPaymentInitiationData
    {
        public string Type { get; set; }
        public IbanityPaymentInitiationAttributes Attributes { get; set; }
    }

    internal class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
