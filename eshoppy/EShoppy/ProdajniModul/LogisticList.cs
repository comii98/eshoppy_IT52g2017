using EShoppy.ProdajniModul.Interfejsi;
using System.Collections.Generic;

namespace EShoppy.ProdajniModul
{
    public static class LogisticList
    {
        /// <summary>
        /// Lista koja ce sadrzati sve ponude u sistemu
        /// </summary>
        public static List<IOffer> ListOffers { get; private set; } = new List<IOffer>();

        /// <summary>
        /// Lista koja ce sadrzati sve proizvode u sistemu
        /// </summary>
        public static List<IProduct> ListProducts { get; private set; } = new List<IProduct>();
    }
}
