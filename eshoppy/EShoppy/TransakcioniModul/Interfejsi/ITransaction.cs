using EShoppy.KorisnickiModul.Interfejsi;
using System;

namespace EShoppy.TransakcioniModul.Interfejsi
{
    public interface ITransaction
    {
        /// <summary>
        /// Interni identifikacioni broj transakcije
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Klijent u transakciji, kupac
        /// </summary>
        IClient Client { get; set; }

        /// <summary>
        /// Cena predmeta transakcije
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Popust na cenu transakcije, vrednost u %
        /// </summary>
        double Discount { get; set; }

        /// <summary>
        /// Datum i vreme transakcije
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Ocena izvrsene transakcije
        /// </summary>
        int Rating { get; set; }
    }
}
