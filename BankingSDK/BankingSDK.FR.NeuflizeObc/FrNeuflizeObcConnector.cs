using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;

namespace BankingSDK.FR.NeuflizeObc
{
    public class FrNeuflizeObcConnector : BaseBerlinGroupConnector
    {
        public FrNeuflizeObcConnector(BankSettings settings) : base(settings, "https://sandbox.neuflizeobc.fr", "https://api.neuflizeobc.fr", ConnectorType.FR_NEUFLIZE_OBC)
        {
        }
    }
}
