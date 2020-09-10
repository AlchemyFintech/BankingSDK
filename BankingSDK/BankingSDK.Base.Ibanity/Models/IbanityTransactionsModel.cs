using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models
{
    internal class IbanityTransactionsModel
    {
        [JsonProperty("data")]
        public List<TransactionDTO> Data { get; set; }
        [JsonProperty("meta")]
        public IbanityMeta Meta { get; set; }
    }

    internal class TransactionModel
    {
        [JsonProperty("data")]
        public TransactionDTO Data { get; set; }
    }

    internal class TransactionDTO
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
    }

    internal class Attributes
    {
        public decimal amount { get; set; }
        public string counterpartName { get; set; }
        public string counterpartReference { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public DateTime executionDate { get; set; }
        public string remittanceInformation { get; set; }
        public string remittanceInformationType { get; set; }
        public DateTime valueDate { get; set; }
    }
}
