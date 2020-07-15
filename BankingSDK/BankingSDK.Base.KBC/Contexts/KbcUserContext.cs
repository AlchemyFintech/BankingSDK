using BankingSDK.Common.Interfaces.Contexts;
using BankingSDK.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSDK.Base.KBC.Contexts
{
    public class KbcUserContext : IUserContext
    {
        public string UserId { get; set; }
        public List<KbcUserConsent> Consents { get; set; } = new List<KbcUserConsent>();
        public List<KbcConsentAccount> Accounts { get; set; } = new List<KbcConsentAccount>();
        List<BaseUserAccount> IUserContext.Accounts { get => Accounts.Cast<BaseUserAccount>().ToList(); set => Accounts = value.Cast<KbcConsentAccount>().ToList(); }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class KbcUserConsent
    {
        public string ConsentId { get; set; }
        public DateTime ValidUntil { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidUntil { get; set; }
    }

    public class KbcConsentAccount : BaseUserAccount
    {
        public string TransactionsConsentId { get; set; }
        public string BalancesConsentId { get; set; }
    }
}
