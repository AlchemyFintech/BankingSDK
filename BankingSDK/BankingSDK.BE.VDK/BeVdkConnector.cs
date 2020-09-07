using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;
using System;

namespace BankingSDK.BE.VDK
{
    public class BeVdkConnector : BaseBerlinGroupConnector
    {
        public BeVdkConnector(BankSettings settings) : base(settings, "https://xs2a-sandbox-api.vdk.be", "https://xs2a-api.vdk.be", ConnectorType.BE_VDK)
        {
        }
    }
}
