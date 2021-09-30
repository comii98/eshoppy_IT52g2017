using EShoppy.FinansijskiModul.Implementacija;
using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.PomocniModul.Implementacija;
using EShoppy.PomocniModul.Interfejsi;
using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EShoppy.KorisnickiModul.Implementacija
{
    /// <summary>
    /// Implementacija logike korisnickog modula
    /// </summary>
    public class ClientManager : IClientManager
    {
        IEmailMessage EmailMessage;
        ILogger ConsoleLogger;
        ILogger FileLogger;
        /// <summary>
        /// Konstruktor klase ClientManager
        /// </summary>
        public ClientManager()
        {
            this.EmailMessage = new EmailMessage();
            this.ConsoleLogger = new ConsoleLogger();
            this.FileLogger = new FileLogger();
        }

        public ClientManager(IEmailMessage EmailMessage)
        {
            this.EmailMessage = EmailMessage;
            this.ConsoleLogger = new ConsoleLogger();
            this.FileLogger = new FileLogger();
        }

        /// <summary>
        /// Method koja prihvata ulazne informacije i kreira fizicko lice u sistemu
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="PhoneNumber"></param>
        /// <param name="BirthDate"></param>
        /// <param name="Address"></param>
        public void RegisterUser(string Name, string Email, string PhoneNumber, DateTime BirthDate, string Address)
        {
            if (string.IsNullOrEmpty(Name)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(Email)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(PhoneNumber)) { throw new ArgumentNullException(); }
            if (BirthDate == null) { throw new ArgumentNullException(); }
            if (BirthDate > DateTime.Today.AddYears(-18)) { throw new ArgumentException(); } // Korisnik mora biti punoletan
            if (string.IsNullOrEmpty(Address)) { throw new ArgumentNullException(); }

            IClient newUser = new User(Name, Email, BirthDate, PhoneNumber, Address, new List<IAccount>());
            // Dodaj korisniku u listu svih korisnika sistema
            ClientList.ListClients.Add(newUser);
        }

        /// <summary>
        /// Method koja prihvata ulazne informacije i kreira pravno lice u sistemu
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="PhoneNumber"></param>
        /// <param name="PIB"></param>
        /// <param name="MB"></param>
        /// <param name="HeadOffice"></param>
        /// <param name="FoundedDate"></param>
        /// <param name="OperatingStatus"></param>
        public void RegisterOrg(string Name, string Email, string PhoneNumber, Int64 PIB, Int64 MB, string HeadOffice, DateTime FoundedDate, string OperatingStatus)
        {
            if (string.IsNullOrEmpty(Name)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(Email)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(PhoneNumber)) { throw new ArgumentNullException(); }
            if (PIB < 0 || PIB > 9999999999) { throw new ArgumentException(); }
            if (MB < 0 || MB > 99999999) { throw new ArgumentException(); }
            if (string.IsNullOrEmpty(HeadOffice)) { throw new ArgumentNullException(); }
            if (FoundedDate > DateTime.Today) { throw new ArgumentException(); }
            if (string.IsNullOrEmpty(OperatingStatus)) { throw new ArgumentNullException(); }

            IClient newOrganization = new Organization(Name, Email, PIB, MB, HeadOffice, FoundedDate, PhoneNumber, OperatingStatus);
            // Dodaj korisniku u listu svih korisnika sistema
            ClientList.ListClients.Add(newOrganization);
        }

        /// <summary>
        /// Metoda koja kao ulazni parametar prihvata objekat klase koja modeluje fizičko lice
        /// i listu njegovih novih računa(jednog ili više) i treba da postojeće brojeve računa zameni novim.
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Accounts"></param>
        public void ChangeUserAccount(IUser User, List<IAccount> Accounts)
        {
            if (User == null) { throw new ArgumentNullException(); }
            if (Accounts == null) { throw new ArgumentNullException(); }
            if (Accounts.Count < 0) { throw new ArgumentException(); };

            IUser UserToUpdate = GetClientByID(User.Id) as User;
            UserToUpdate.ListAccounts = Accounts;
        }

        /// <summary>
        /// Metoda koja kao ulazni parametar prihvata objekat klase koja modeluje pravno lice i
        /// listu njegovih novih računa(jednog ili više) i treba da postojeće brojeve računa zameni novim.
        /// </summary>
        /// <param name="Organization"></param>
        /// <param name="Accounts"></param>
        public void ChangeOrgAccount(IOrganization Organization, List<IAccount> Accounts)
        {
            if (Organization == null) { throw new ArgumentNullException(); }
            if (Accounts == null) { throw new ArgumentNullException(); }
            if (Accounts.Count < 0) { throw new ArgumentException(); };

            IOrganization OrganizationToUpdate = GetClientByID(Organization.Id) as Organization;
            OrganizationToUpdate.ListAccounts = Accounts;
        }

        public List<ITransaction> SearchHistory(IClient Client, DateTime? Date, int? TransactionType, int? Rating)
        {
            if (Client == null) { throw new ArgumentNullException(); }

            IClient ClientToFind = ClientList.ListClients.FirstOrDefault(c => c.Id == Client.Id);

            if (TransactionType == null)
            {
                // Purchase and Sale
                var PurchaseTransactionList = ClientToFind.PurchaseTransaction.Where(t => t.CreatedAt > Date && t.Rating > Rating).ToList();
                var SaleTransactionList = ClientToFind.SaleTransaction.Where(t => t.CreatedAt > Date && t.Rating > Rating).ToList();

                var AllTransactionList = PurchaseTransactionList.Union(SaleTransactionList).ToList();

                return AllTransactionList;
            }
            else
            {
                // 0 – kupovne transakcije, 1 – prodajne transakcije
                // Purchase
                if (TransactionType == null || TransactionType == 0)
                {
                    return ClientToFind.PurchaseTransaction.Where(t => t.CreatedAt > Date && t.Rating > Rating).ToList();
                }
                // Sale
                else
                {
                    return ClientToFind.SaleTransaction.Where(t => t.CreatedAt > Date && t.Rating > Rating).ToList();
                }
            }
        }

        /// <summary>
        /// Mmetoda koja kao ulazni parametar treba da prihvati ID klijenta i da iz liste svih 
        /// korisnika koja se nalazi u klasi Client pronadje korisnika sa prosleđenim ID-em.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public IClient GetClientByID(Guid ClientId)
        {
            if (ClientId == null) { throw new ArgumentNullException(); }
            IClient Client = ClientList.ListClients.FirstOrDefault(u => u.Id == ClientId);

            if (Client == null)
            {
                throw new ArgumentException($"Klijent sa internim identifikacionim brojem: {ClientId}, ne postoji u bazi.");
            }

            return Client;
        }

        /// <summary>
        /// Metoda koja će kao ulazne parametre prihvatati klijenta, količinu novca i
        /// valutu u kojoj se novac uplaćuje na korisnikov račun.
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="AccountId"></param>
        /// <param name="Amount"></param>
        /// <param name="Currency"></param>
        public void AddFunds(IClient Client, Guid AccountId, decimal Amount, Currency Currency)
        {
            if (Client == null) { throw new ArgumentNullException(); }
            if (AccountId == null) { throw new ArgumentNullException(); }
            if (Amount < 0) { throw new ArgumentException(); };
            if (Currency == null) { throw new ArgumentNullException(); }

            var AccountToUpdate = Client.ListAccounts.FirstOrDefault(a => a.Id == AccountId);
            if (AccountToUpdate == null)
            {
                throw new ArgumentException($"Ne postoji racun broj: {AccountId}, za klijenta {Client.Name}.");
            }
            var ConvertedAmount = Currency.Convert(Amount);

            // Ukoliko je iznos uplate na račun manji od 10000 u dinarskoj valuti i reč je o pravnom licu, uplata se odbija i treba logovati poruku o grešci
            if (ConvertedAmount < 10000 && Client is Organization)
            {
                ConsoleLogger.LogMessage($"Uplata se odbija, iznos uplate {ConvertedAmount} je manji od 10.000,00 RSD, a rec je o pravnom licu {Client.Name}.");
                FileLogger.LogMessage($"Uplata se odbija, iznos uplate {ConvertedAmount} je manji od 10.000,00 RSD, a rec je o pravnom licu {Client.Name}.");
            }

            IFinanceManager FinanceManager = new FinanceManager();
            // Ukoliko ima kredit, vrše se sledeći koraci
            if (AccountToUpdate.CreditAvailable)
            {
                // Umanjivanje kreditnog zaduženja za prosleđeni iznos
                FinanceManager.CreditPayment(AccountToUpdate.Id, ConvertedAmount);
                // Slanje mejla o smanjenju kreditnog zaduženja korisniku
                EmailMessage.SendEmail(Client.Email, "Uplacena rata kredita", $"Postovani, obavestavamo Vas da nam je pristigla rata kredita u iznosu od: {ConvertedAmount}.");
            }
            // Ukoliko korisnik nema kredit, vrši se uplata na njegov račun
            else
            {
                FinanceManager.AccountPayment(AccountToUpdate.Id, ConvertedAmount);
                // Slanje mejla o uplati na racun korisnika
                EmailMessage.SendEmail(Client.Email, "Priliv na racun", $"Postovani, obavestavamo Vas da Vam je pristigao priliv na racun {AccountToUpdate.Number} u iznosu od: {ConvertedAmount}.");
            }
        }
    }
}
