using BankingSDK.Base.BerlinGroup;
using BankingSDK.Common;
using BankingSDK.Common.Enums;
using System;

namespace BankingSDK.BE.CPH
{
    public class BeCphConnector : BaseBerlinGroupConnector
    {
        public BeCphConnector(BankSettings settings) : base(settings, "https://sandbox.psd2.cph.be", "https://api.psd2.cph.be", ConnectorType.BE_CPH)
        {
        }
    }
}
