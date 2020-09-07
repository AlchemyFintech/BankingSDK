using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;

namespace BankingSDK.FR.LazardFreresBanque
{
    public class FrLazardFreresBanqueConnector : BaseBerlinGroupConnector
    {
        public FrLazardFreresBanqueConnector(BankSettings settings) : base(settings, "https://sandbox.psd2.lazardfreresbanque.fr", "https://api.psd2.lazardfreresbanque.fr", ConnectorType.FR_LAZARD_FRERES_BANQUE)
        {
        }
    }
}
