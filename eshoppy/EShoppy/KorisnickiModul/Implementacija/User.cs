using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.KorisnickiModul.Implementacija
{
    /// <summary>
    /// Klasa koja opisuje klienta: Fizicko lice
    /// </summary>
    public class User : IUser
    {
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public List<ITransaction> PurchaseTransaction { get; set; }
        public List<ITransaction> SaleTransaction { get; set; }

        public List<IAccount> ListAccounts { get; set; }

        public User(string Name, string Email, DateTime BirthDate, string PhoneNumber, string Address, List<IAccount> Accounts)
        {
            this.Name = Name;
            this.Email = Email;
            this.BirthDate = BirthDate;
            this.PhoneNumber = PhoneNumber;
            this.Address = Address;

            this.Id = Guid.NewGuid();
            this.Password = Guid.NewGuid().ToString("d").Substring(1, 8);

            this.ListAccounts = Accounts;
            this.PurchaseTransaction = new List<ITransaction>();
            this.SaleTransaction = new List<ITransaction>();
        }
    }
}
