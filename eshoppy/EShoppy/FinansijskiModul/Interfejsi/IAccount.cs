using System;

namespace EShoppy.FinansijskiModul.Interfejsi
{
    public interface IAccount
    {
        /// <summary>
        /// Interni identifikacioni broj racuna
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Broj racuna
        /// </summary>
        string Number { get; set; }

        /// <summary>
        /// Stanje racuna
        /// </summary>
        decimal Balance { get; set; }

        /// <summary>
        /// Kreditno zaduzenje 
        /// </summary>
        decimal CreditPayment { get; set; }

        /// <summary>
        /// Da li je dostupan kredit
        /// </summary>
        bool CreditAvailable { get; set; }

        /// <summary>
        /// Banka u kojoj se otvara racun
        /// </summary>
        IBank Bank { get; set; }
    }
}
