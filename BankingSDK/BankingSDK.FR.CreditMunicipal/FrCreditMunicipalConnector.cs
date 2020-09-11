using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;
using System;

namespace BankingSDK.FR.CreditMunicipal
{
    public class FrCreditMunicipalConnector : BaseBerlinGroupConnector
    {
        public FrCreditMunicipalConnector(BankSettings settings) : base(settings, "https://sandbox.ccmdirect.fr", "https://api.ccmdirect.fr", ConnectorType.FR_CREDIT_MUNICIPAL)
        {
        }
    }
}
