using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;

namespace BankingSDK.DE.CommerzBank
{
    public class DeCommerzBankConnector : BaseBerlinGroupConnector
    {
        public DeCommerzBankConnector(BankSettings settings) : base(settings, "https://psd2.api-sandbox.commerzbank.com", "https://psd2.api.commerzbank.com", ConnectorType.DE_COMMERZ_BANK)
        {
        }
    }
}
