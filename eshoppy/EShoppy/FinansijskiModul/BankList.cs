using EShoppy.FinansijskiModul.Interfejsi;
using System.Collections.Generic;

namespace EShoppy.FinansijskiModul
{
    public static class BankList
    {
        /// <summary>
        /// Listu svih banaka dodatih u sistem
        /// </summary>
        public static List<IBank> ListBanks { get; private set; } = new List<IBank>();
    }
}
