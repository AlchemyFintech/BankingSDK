using BankingSDK.Common.Interfaces.Contexts;
using BankingSDK.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSDK.BE.Belfius.Contexts
{
    public class BelfiusUserContext : IUserContext
    {
        public string UserId { get; set; }
        public List<ConsentAccount> Accounts { get; set; } = new List<ConsentAccount>();
        List<BaseUserAccount> IUserContext.Accounts { get => Accounts.Cast<BaseUserAccount>().ToList(); set => Accounts = value.Cast<ConsentAccount>().ToList(); }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ConsentAccount : BaseUserAccount
    {
        public UserToken Token { get; set; }
    }

    public class UserToken
    {
        public DateTime ValidUntil { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidUntil { get; set; }
    }
}
