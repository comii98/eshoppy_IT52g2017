using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul;
using EShoppy.KorisnickiModul.Implementacija;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.PomocniModul.Implementacija;
using EShoppy.PomocniModul.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class FinanceManager : IFinanceManager
    {
        IEmailMessage EmailMessage;
        ILogger ConsoleLogger;
        ILogger FileLogger;

        public FinanceManager()
        {
            EmailMessage = new EmailMessage();
            ConsoleLogger = new ConsoleLogger();
            FileLogger = new FileLogger();
        }

        public FinanceManager(IEmailMessage EmailMessage)
        {
            this.EmailMessage = EmailMessage;
            this.ConsoleLogger = new ConsoleLogger();
            this.FileLogger = new FileLogger();
        }

        public void AccountPayment(Guid AccountId, decimal Amount)
        {
            IAccount Account = GetAccountByID(AccountId);
            // Umanjivanje kreditnog zaduženja za prosleđeni iznos
            Account.Balance += Amount;
        }

        public void AskCredit(Guid ClientId, Guid CreditId, int PaymentPeriod, decimal Amount)
        {
            IClientManager ClientManager = new ClientManager();
            IClient Client = ClientManager.GetClientByID(ClientId);

            if (Client.ListAccounts.Count < 1)
            {
                ConsoleLogger.LogMessage($"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema otvoren racun u banci.");
                FileLogger.LogMessage($"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema otvoren racun u banci.");
                throw new KeyNotFoundException($"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema otvoren racun u banci.");
            }

            IAccount AccountForCredit = Client.ListAccounts.FirstOrDefault(a => a.CreditAvailable);
            if (AccountForCredit == null)
            {
                ConsoleLogger.LogMessage($"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema odobrenje za kredit na bilo kom racunu.");
                FileLogger.LogMessage($"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema odobrenje za kredit na bilo kom racunu.");
                EmailMessage.SendEmail(Client.Email, "Kredit odbijen", $"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema odobrenje za kredit na bilo kom racunu.");
                throw new KeyNotFoundException($"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema odobrenje za kredit na bilo kom racunu.");
            }

            ICredit Credit = AccountForCredit.Bank.ListOfCredits.FirstOrDefault(c => c.Id == CreditId);
            if (Credit == null)
            {
                ConsoleLogger.LogMessage($"Kredit id {CreditId}, nije dostupan u banci {AccountForCredit.Bank.Name}.");
                FileLogger.LogMessage($"Kredit id {CreditId}, nije dostupan u banci {AccountForCredit.Bank.Name}.");
                EmailMessage.SendEmail(Client.Email, "Kredit odbijen", $"Kredit id {CreditId}, nije dostupan u banci {AccountForCredit.Bank.Name}.");
                throw new KeyNotFoundException($"Kredit id {CreditId}, nije dostupan u banci {AccountForCredit.Bank.Name}.");
            }

            if (PaymentPeriod * 12 > Credit.MaxCreditMonths)
            {
                ConsoleLogger.LogMessage($"Kredit {Credit.Name}, se moze uzeti na maksimalno {Credit.MaxCreditMonths} meseci, odnosno {Credit.MaxCreditMonths / 12} godina.");
                FileLogger.LogMessage($"Kredit {Credit.Name}, se moze uzeti na maksimalno {Credit.MaxCreditMonths} meseci, odnosno {Credit.MaxCreditMonths / 12} godina.");
                EmailMessage.SendEmail(Client.Email, "Kredit odbijen", $"Kredit {Credit.Name}, se moze uzeti na maksimalno {Credit.MaxCreditMonths} meseci, odnosno {Credit.MaxCreditMonths / 12} godina.");
                throw new ArgumentException($"Kredit {Credit.Name}, se moze uzeti na maksimalno {Credit.MaxCreditMonths} meseci, odnosno {Credit.MaxCreditMonths / 12} godina.");
            }

            if (Amount > Credit.MaxCreditAmount)
            {
                ConsoleLogger.LogMessage($"Kredit {Credit.Name}, ima limit od {Credit.MaxCreditAmount} dinara.");
                FileLogger.LogMessage($"Kredit {Credit.Name}, ima limit od {Credit.MaxCreditAmount} dinara.");
                EmailMessage.SendEmail(Client.Email, "Kredit odbijen", $"Kredit {Credit.Name}, ima limit od {Credit.MaxCreditAmount} dinara.");
                throw new ArgumentException($"Kredit {Credit.Name}, ima limit od {Credit.MaxCreditAmount} dinara.");
            }

            EmailMessage.SendEmail(Client.Email, "Kredit odobren", $"Cestitamo, kredit {Credit.Name} Vam je odobren, zatrazeni iznos {Amount} ce uskoro biti na Vasem racunu.");
            AccountForCredit.Balance += Amount;
            var Interest = Amount * (decimal)Credit.MonthInterestRate * PaymentPeriod * 12;
            AccountForCredit.CreditPayment += Amount + Interest;
        }

        public decimal CheckBalance(Guid AccountId)
        {
            IAccount Account = GetAccountByID(AccountId);
            // Umanjivanje kreditnog zaduženja za prosleđeni iznos
            return Account.Balance;
        }

        public decimal Convert(Currency Currency, decimal Amount)
        {
            if (Currency == null) { throw new ArgumentNullException(); }
            return Currency.Convert(Amount);
        }

        public void CreateAccount(string Number, bool CreditAvailable, IBank Bank, IClient Client)
        {
            if (string.IsNullOrEmpty(Number)) { throw new ArgumentNullException(); }
            if (Bank == null) { throw new ArgumentNullException(); }
            if (Client == null) { throw new ArgumentNullException(); }

            var Balance = 0M;
            var CreditPayment = 0M;
            IAccount Account = new Account(Number, Balance, CreditAvailable, CreditPayment, Bank, Client);

            Client.ListAccounts.Add(Account);
        }

        public void CreateBank(string Name, string HeadOffice, string AccountPrefix)
        {
            if (string.IsNullOrEmpty(Name)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(HeadOffice)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(AccountPrefix)) { throw new ArgumentNullException(); }
            IBank Bank = new Bank(Name, HeadOffice, AccountPrefix);

            BankList.ListBanks.Add(Bank);
        }

        public void CreateCredit(string Name, decimal MaxCreditAmount, double MonthInterestRate, int MinCreditMonths, int MaxCreditMonths, Currency Currency, IBank Bank)
        {
            if (string.IsNullOrEmpty(Name)) { throw new ArgumentNullException(); }
            if (Currency == null) { throw new ArgumentNullException(); }
            if (Bank == null) { throw new ArgumentNullException(); }
            if (MaxCreditAmount < 0) { throw new ArgumentException(); };
            if (MonthInterestRate < 0) { throw new ArgumentException(); };
            if (MinCreditMonths < 12) { throw new ArgumentException(); };
            if (MaxCreditMonths > 360) { throw new ArgumentException(); };
            if (MinCreditMonths > MaxCreditMonths) { throw new ArgumentException(); };

            ICredit Credit = new Credit(Name, MaxCreditAmount, Currency, MonthInterestRate, MinCreditMonths, MaxCreditMonths, Bank);

            Bank.ListOfCredits.Add(Credit);
        }

        public void CreditPayment(Guid AccountId, decimal Amount)
        {
            if (Amount < 0) { throw new ArgumentException(); };
            IAccount Account = GetAccountByID(AccountId);
            // Umanjivanje kreditnog zaduženja za prosleđeni iznos
            Account.CreditPayment -= Amount;
        }

        public IAccount GetAccountByID(Guid AccountId)
        {
            if (AccountId == null) { throw new ArgumentNullException(); }
            var Account = ClientList.ListClients.SelectMany(c => c.ListAccounts.Where(a => a.Id == AccountId)).FirstOrDefault();
            
            if (Account == null)
            {
                throw new ArgumentException($"Bankovni racun sa internim identifikacionim brojem: {AccountId}, ne postoji u bazi.");
            }

            return Account;
        }
    }
}
