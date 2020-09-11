using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;

namespace BankingSDK.DE.Comdirect
{
    public class DeComdirectConnector : BaseBerlinGroupConnector
    {
        public DeComdirectConnector(BankSettings settings) : base(settings, "https://xs2a-sandbox.comdirect.de", "https://xs2a-api.comdirect.de", ConnectorType.DE_COM_DIRECT)
        {
        }
    }
}
