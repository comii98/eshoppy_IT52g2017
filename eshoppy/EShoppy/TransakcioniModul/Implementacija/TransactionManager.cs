using EShoppy.FinansijskiModul.Implementacija;
using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul.Implementacija;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.PomocniModul.Implementacija;
using EShoppy.PomocniModul.Interfejsi;
using EShoppy.ProdajniModul;
using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EShoppy.TransakcioniModul.Implementacija
{
    public class TransactionManager : ITransactionManager
    {
        IClientManager ClientManager;
        ITransaction Transaction;
        IFinanceManager FinanceManager;
        IEmailMessage EmailMessage;
        ILogger ConsoleLogger;
        ILogger FileLogger;

        public TransactionManager()
        {
            this.ClientManager = new ClientManager();
            this.Transaction = new Transaction();
            this.FinanceManager = new FinanceManager();
            this.EmailMessage = new EmailMessage();
            this.ConsoleLogger = new ConsoleLogger();
            this.FileLogger = new FileLogger();
        }

        public TransactionManager(IEmailMessage EmailMessage)
        {
            this.EmailMessage = EmailMessage;
            this.ClientManager = new ClientManager();
            this.Transaction = new Transaction();
            this.FinanceManager = new FinanceManager();
            this.ConsoleLogger = new ConsoleLogger();
            this.FileLogger = new FileLogger();
        }

        public void CreateTransaction(Guid ClientId, Guid OrganizationId, Guid OfferId, Guid TransactionTypeId, int Rating)
        {
            IClient Client = ClientManager.GetClientByID(ClientId);
            IClient Organization = ClientManager.GetClientByID(OrganizationId);

            var Offer = LogisticList.ListOffers.FirstOrDefault(o => o.Id == OfferId);
            if (Offer == null)
            {
                throw new KeyNotFoundException($"Ne postoji ponuda sa id {OfferId} u sistemu.");
            }
            
            ITransactionType TransactionType = TransactionList.TransactionTypes.FirstOrDefault(t => t.Id == TransactionTypeId);
            if (TransactionType == null)
            {
                throw new KeyNotFoundException($"U sistemu ne postoji tip transakcije sa id {TransactionTypeId}.");
            }

            Transaction.Id = Guid.NewGuid();
            Transaction.Price = Offer.Price;
            Transaction.CreatedAt = DateTime.Now;
            Transaction.Rating = Rating;
            Transaction.Client = Client;

            // Ukoliko je ponuda starija od 6 meseci, korisnik dobija popust od 12%
            DateTime BeforeSixMonth = DateTime.Today.AddMonths(-6);
            if (BeforeSixMonth > Offer.CreatedAt)
            {
                Transaction.Discount += 12;
            }

            // Ukoliko se transakcija izvršava u decembru i januaru (oko novogodišnjih) praznika, korisnik dobija popust od 5 %
            if (Transaction.CreatedAt.Month == 12 || Transaction.CreatedAt.Month == 1)
            {
                Transaction.Discount += 5;
            }

            // Ukoliko je broj proizvoda u okviru ponude veći od 3, korisnik dobija popust od 5%
            if (Offer.OfferProducts.Count > 3)
            {
                Transaction.Discount += 5;
            }

            decimal PriceWithDiscount = Transaction.Price - Transaction.Price * (decimal)Transaction.Discount / 100;

            // Pun iznos ili prva rata definisana u listi delova transakcije
            // decimal PriceToPay = (decimal)TransactionType.TransactionPieces.FirstOrDefault() * PriceWithDiscount;

            IAccount ClientAccount = Client.ListAccounts.FirstOrDefault();
            if (ClientAccount == null)
            {
                throw new KeyNotFoundException($"Klijent {Client.Name}, nema dostupnih racuna u sistemu da bi izvrsio placanje.");
            }

            IAccount OrganizationAccount = Organization.ListAccounts.FirstOrDefault();
            if (OrganizationAccount == null)
            {
                throw new KeyNotFoundException($"Klijent {Organization.Name}, nema dostupnih racuna u sistemu da bi naplatio ponudu.");
            }

            decimal ClientBalance = ClientAccount.Balance;
            if (PriceWithDiscount < ClientBalance)
            {
                var TransactionPieceToPay = TransactionType.TransactionPieces.FirstOrDefault();
                // Placanje u celosti
                if (TransactionPieceToPay == 1)
                {
                    // Umanjivanje stanja na korisničkom računu za celokupan iznos transakcije(umanjen za popust)
                    FinanceManager.AccountPayment(ClientAccount.Id, -PriceWithDiscount);

                    // Uvećavanje stanja na računu prodavca za celokupan iznos transakcije (umanjen za popust)
                    FinanceManager.AccountPayment(OrganizationAccount.Id, PriceWithDiscount);
                }
                else
                {
                    var PriceToPay = PriceWithDiscount * (decimal)TransactionPieceToPay;
                    // Umanjivanje stanja na korisničkom računu za iznos prve rate transakcije(umanjen za popust)
                    FinanceManager.AccountPayment(ClientAccount.Id, -PriceWithDiscount);

                    // Uvećanje stanja na računu prodavca za iznos prve rate transakcije (umanjen za popust).
                    FinanceManager.AccountPayment(OrganizationAccount.Id, PriceWithDiscount);
                }

                Client.PurchaseTransaction.Add(Transaction);
                Organization.PurchaseTransaction.Add(Transaction);
                TransactionList.ListTransactions.Add(Transaction);

                ConsoleLogger.LogMessage($"Transakcija uspesna. Uspesno izvrsena transakcija {Transaction.Id}, za ponudu {Offer.Id}.");
                FileLogger.LogMessage($"Transakcija uspesna. Uspesno izvrsena transakcija {Transaction.Id}, za ponudu {Offer.Id}.");
                // Slanje mejla korisniku o potvrdi transakcije.
                EmailMessage.SendEmail(Client.Email, "Transakcija uspesna", $"Uspesno izvrsena transakcija {Transaction.Id}, za ponudu {Offer.Id}.");
                EmailMessage.SendEmail(Organization.Email, "Transakcija uspesna", $"Uspesno izvrsena transakcija {Transaction.Id}, za ponudu {Offer.Id}.");
            }
            else
            {
                ConsoleLogger.LogMessage($"Transakcija neuspesna, klijent {Client.Name} nema dovoljno novca na racunu da se izvrsi transakcija.");
                FileLogger.LogMessage($"Transakcija neuspesna, klijent {Client.Name} nema dovoljno novca na racunu da se izvrsi transakcija.");
                EmailMessage.SendEmail(Client.Email, "Transakcija neuspesna", $"Nemate dovoljno novca na racunu da se izvrsi transakcija.");
            }
        }
    }
}
