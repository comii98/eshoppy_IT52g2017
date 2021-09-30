using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.KorisnickiModul.Interfejsi
{
    /// <summary>
    /// Interfejs za sve tipove klijenata sa zajednickim atributima
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Genericki id korisnika
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Ime i prezime fizickog lica/Naziv pravnog lica
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Email korisnickog naloga
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Sifra za pristup korisnickom nalogu
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Broj telefona klijenta
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// Lista korisnicnih bankarskih racuna
        /// </summary>
        List<IAccount> ListAccounts { get; set; }

        /// <summary>
        /// Kupovne transakcije
        /// </summary>
        List<ITransaction> PurchaseTransaction { get; set; }

        /// <summary>
        /// Prodajne transakcije
        /// </summary>
        List<ITransaction> SaleTransaction { get; set; }
    }
}
