using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;

namespace BankingSDK.BE.Aion
{
    public class BeAionConnector : BaseBerlinGroupConnector
    {
        public BeAionConnector(BankSettings settings) : base(settings, "https://sandbox.psd2.aion.be", "https://api.psd2.aion.be", ConnectorType.BE_AION)
        {
        }
    }
}
