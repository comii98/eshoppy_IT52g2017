using EShoppy.FinansijskiModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class Bank : IBank
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string HeadOffice { get; set; }
        public string AccountPrefix { get; set; }
        public List<ICredit> ListOfCredits { get; set; }
        public List<IAccount> ListOfAccounts { get; set; }

        public Bank(string Name, string HeadOffice, string AccountPrefix)
        {
            this.Id = Guid.NewGuid();
            this.Name = Name;
            this.HeadOffice = HeadOffice;
            this.AccountPrefix = AccountPrefix;

            this.ListOfCredits = new List<ICredit>();
            this.ListOfAccounts = new List<IAccount>();
        }
    }
}
