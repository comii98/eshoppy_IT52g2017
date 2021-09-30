using EShoppy.TransakcioniModul.Interfejsi;
using System.Collections.Generic;

namespace EShoppy.TransakcioniModul
{
    public static class TransactionList
    {
        /// <summary>
        /// Lista svih transakcija u sistema i koristiće se za pretragu istih
        /// </summary>
        public static List<ITransaction> ListTransactions { get; private set; } = new List<ITransaction>();

        /// <summary>
        /// List svih tipova transakcija u sistemu (Placanje u celosti i placanje na rate)
        /// </summary>
        public static List<ITransactionType> TransactionTypes { get; private set; } = new List<ITransactionType>();
    }
}
