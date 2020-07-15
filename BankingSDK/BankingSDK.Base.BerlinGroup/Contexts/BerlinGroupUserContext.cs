using BankingSDK.Common.Interfaces.Contexts;
using BankingSDK.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSDK.Base.BerlinGroup.Contexts
{
    public class BerlinGroupUserContext : IUserContext
    {
        public string UserId { get; set; }
        public List<BerlinGroupUserConsent> Consents { get; set; } = new List<BerlinGroupUserConsent>();
        public List<BerlinGroupConsentAccount> Accounts { get; set; } = new List<BerlinGroupConsentAccount>();
        List<BaseUserAccount> IUserContext.Accounts { get => Accounts.Cast<BaseUserAccount>().ToList(); set => Accounts = value.Cast<BerlinGroupConsentAccount>().ToList(); }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class BerlinGroupUserConsent
    {
        public string ConsentId { get; set; }
        public DateTime ValidUntil { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidUntil { get; set; }
    }

    public class BerlinGroupConsentAccount : BaseUserAccount
    {
        public string TransactionsConsentId { get; set; }
        public string BalancesConsentId { get; set; }
    }
}
