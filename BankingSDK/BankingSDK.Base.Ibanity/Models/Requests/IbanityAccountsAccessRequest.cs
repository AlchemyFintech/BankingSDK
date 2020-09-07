using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSDK.Base.Ibanity.Models.Requests
{
    public class IbanityAccountsAccessRequest
    {
        public IbanityAccountInformationAccessData Data { get; set; }
    }

    public class IbanityAccountInformationAccessAttributes
    {
        public string RedirectUri { get; set; }
        public string ConsentReference { get; set; }
        public List<string> RequestedAccountReferences { get; set; }
        public string Locale { get; set; }
        public string CustomerIpAddress { get; set; }
    }

    public class IbanityAuthorizationPortal
    {
        public string FinancialInstitutionPrimaryColor { get; set; }
        public string FinancialInstitutionSecondaryColor { get; set; }
        public string DisclaimerTitle { get; set; }
        public string DisclaimerContent { get; set; }
    }

    public class IbanityAccountInformationAccessMeta
    {
        public IbanityAuthorizationPortal AuthorizationPortal { get; set; }
    }

    public class IbanityAccountInformationAccessData
    {
        public string Type { get; set; }
        public IbanityAccountInformationAccessAttributes Attributes { get; set; }
        public IbanityAccountInformationAccessMeta Meta { get; set; }
    }
}
