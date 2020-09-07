using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;

namespace BankingSDK.NL.CreditEurope
{
    public class NlCreditEuropeConnector : BaseBerlinGroupConnector
    {
        public NlCreditEuropeConnector(BankSettings settings) : base(settings, "https://tppsandbox.crediteurope.nl", "https://psapi.crediteurope.nl", ConnectorType.NL_CREDIT_EUROPE)
        {
        }
    }
}
