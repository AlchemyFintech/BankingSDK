using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;
using System;

namespace BankingSDK.FR.OrangeBank
{
    public class FrOrangeBankConnector : BaseBerlinGroupConnector
    {
        public FrOrangeBankConnector(BankSettings settings) : base(settings, "https://sandbox-api-tpp.orangebank.fr", "https://api-tpp.orangebank.fr", ConnectorType.FR_ORANGE_BANK)
        {
        }
    }
}
