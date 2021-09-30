using System;
using System.Collections.Generic;

namespace EShoppy.TransakcioniModul.Interfejsi
{
    public interface ITransactionType
    {
        /// <summary>
        /// Interni identifikacioni broj transakcije
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Naziv nacina transakcije
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Opis nacina transakcije
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Delovi transakcije, definisani listom, Pun iznos = {1}, Placanje na 3 rate = {0.33, 0.33, 0.33}
        /// </summary>
        List<double> TransactionPieces { get; set; }
    }
}
