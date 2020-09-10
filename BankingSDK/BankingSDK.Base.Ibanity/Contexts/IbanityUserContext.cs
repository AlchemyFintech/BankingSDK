using BankingSDK.Common.Interfaces.Contexts;
using BankingSDK.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingSDK.Base.Ibanity.Contexts
{
    public class IbanityUserContext : IUserContext
    {
        public string UserId { get; set; }
        public Guid ContextId { get; set; }
        public string Token { get; set; }
        public List<UserAccount> Accounts { get; set; } = new List<UserAccount>();
        List<BaseUserAccount> IUserContext.Accounts { get => Accounts.Cast<BaseUserAccount>().ToList(); set => Accounts = value.Cast<UserAccount>().ToList(); }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserAccessToken
    {
        [JsonIgnore]
        public string FullToken
        {
            get
            {
                return $"{TokenType} {AccessToken}";
            }
        }

        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenValidUntil { get; set; }
        public string RefreshAccessToken { get; set; }
        public DateTime RefreshTokenValidUntil { get; set; }
    }

    public class UserAccount : BaseUserAccount
    {
        public string RefreshAccessToken { get; set; }
    }
}
