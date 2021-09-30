using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.KorisnickiModul.Implementacija
{
    /// <summary>
    /// Klasa koja opisuje pravno lice
    /// </summary>
    public class Organization : IOrganization
    {
        public Int64 PIB { get; set; }
        public Int64 MB { get; set; }
        public string HeadOffice { get; set; }
        public DateTime FoundedDate { get; set; }
        public string OperatingStatus { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public List<IAccount> ListAccounts { get; set; }
        public List<ITransaction> PurchaseTransaction { get; set; }
        public List<ITransaction> SaleTransaction { get; set; }

        public Organization(string Name, string Email, Int64 PIB, Int64 MB, string HeadOffice, DateTime FoundedDate, string PhoneNumber, string OperatingStatus)
        {
            this.Name = Name;
            this.Email = Email;
            this.PIB = PIB;
            this.MB = MB;
            this.HeadOffice = HeadOffice;
            this.FoundedDate = FoundedDate;
            this.PhoneNumber = PhoneNumber;
            this.OperatingStatus = OperatingStatus;

            this.ListAccounts = new List<IAccount>();
            this.PurchaseTransaction = new List<ITransaction>();
            this.SaleTransaction = new List<ITransaction>();
        }
    }
}
