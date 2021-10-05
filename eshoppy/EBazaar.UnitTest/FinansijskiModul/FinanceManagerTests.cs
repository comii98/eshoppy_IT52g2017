using System;
using System.Collections.Generic;
using EShoppy.FinansijskiModul;
using EShoppy.FinansijskiModul.Implementacija;
using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul;
using EShoppy.KorisnickiModul.Implementacija;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.PomocniModul.Interfejsi;
using EShoppy.ProdajniModul;
using EShoppy.ProdajniModul.Implementacija;
using EShoppy.ProdajniModul.Interfejsi;
using EShoppy.TransakcioniModul;
using EShoppy.TransakcioniModul.Implementacija;
using EShoppy.TransakcioniModul.Interfejsi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EBazaar.UnitTest.FinansijskiModul
{
    [TestClass]
    public class FinanceManagerTests
    {
        IClientManager ClientManager;
        ITransactionManager TransactionManager;
        IFinanceManager FinanceManager;
        Mock<IEmailMessage> EmailMessageMock;
        ILogger ConsoleLogger;
        ILogger FileLogger;
        IClient Client;
        IClient Organization;
        IAccount Account;
        IAccount OrganizationAccount;
        IBank Bank;
        ICredit Credit;
        IOffer Offer;
        IProduct Product;
        ITransport Transport;
        ITransactionType TransactionType;

        [TestInitialize]
        public void TestInitialize()
        {
            Client = new User("Marko Markovic", "markom@gmail.com", DateTime.Today.AddYears(-20), "+38121000000", "Dunavska 111, 21000 Novi Sad", new List<IAccount>());
            Organization = new Organization("Delta Holding", "office@deltaholding.rs", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "+38163000000", "Aktivno");
            Bank = new Bank("Banca Intesa", "Milentija Popovica, 11100 Novi Beograd", "165");
            Credit = new Credit("Kredit", 1000000, new DinarCurrency(), 10, 6, 120, Bank);
            Account = new Account("11223344", 0, false, 0, Bank, Client);
            OrganizationAccount = new Account("00110011", 0, false, 0, Bank, Organization);
            Product = new Product("Proizvod 1", "Opis proizvoda 1", 100000, false);
            Transport = new Transport("Transport 1", 2);
            Offer = new Offer(Organization, new List<IProduct> { Product }, new List<ITransport> { Transport }, 200000, DateTime.Now.AddMonths(-7));
            TransactionType = new TransactionType("Placanje u celosti", "Placanje u celosti ponude sa uracunatim popustima", new List<double> { 1 });
            //AccountWithCredit = new Account("44332211", 0, true, 117700, Bank, Organization);
            EmailMessageMock = new Mock<IEmailMessage>();
            TransactionManager = new TransactionManager(EmailMessageMock.Object);
            FinanceManager = new FinanceManager(EmailMessageMock.Object);
            ClientManager = new ClientManager(EmailMessageMock.Object);
        }

        [TestMethod]
        public void CreateAccount_ValidTest()
        {
            Assert.IsTrue(Client.ListAccounts.Count == 0);
            FinanceManager.CreateAccount("12321321312", false, Bank, Client);
            Assert.IsTrue(Client.ListAccounts.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateAccount_InvalidTestBankNull()
        {
            FinanceManager.CreateAccount("12321321312", false, null, Client);
        }

        [TestMethod]
        public void CreateBank_ValidTest()
        {
            BankList.ListBanks.Clear();
            FinanceManager.CreateBank("Banka", "Adresa Novi Sad", "165");
            Assert.AreEqual(1, BankList.ListBanks.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateAccount_InvalidTestNameEmpty()
        {
            FinanceManager.CreateBank("", "Adresa Novi Sad", "165");
        }

        [TestMethod]
        public void CreateCredit_ValidTest()
        {
            Bank.ListOfCredits.Clear();
            FinanceManager.CreateCredit("Credit Name", 100000, 12, 12, 360, new EuroCurrency(), Bank);
            Assert.IsTrue(Bank.ListOfCredits.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCredit_InvalidTestNameEmpty()
        {
            FinanceManager.CreateCredit("", 100000, 12, 10, 400, new EuroCurrency(), Bank);
        }

        [TestMethod]
        public void GetAccountById_ValidTest()
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            var AccountResult = FinanceManager.GetAccountByID(Account.Id);
            Assert.IsTrue(AccountResult == Account);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAccountById_InvalidTestAccountId()
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            FinanceManager.GetAccountByID(Guid.NewGuid());
        }

        [TestMethod]
        public void AccountPayment_ValidTest()
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            FinanceManager.AccountPayment(Account.Id, 1);
            Assert.AreEqual(1, Account.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AccountPayment_InvalidTestAccountId()
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            FinanceManager.AccountPayment(Guid.NewGuid(), 500);
        }

        [TestMethod]
        public void CreditPayment_ValidTest()
        {
            ClientList.ListClients.Add(Client);
            Account.CreditPayment = 100;
            Client.ListAccounts.Add(Account);
            FinanceManager.CreditPayment(Account.Id, 100);
            Assert.AreEqual(0, Account.CreditPayment);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreditPayment_InvalidTestAccountId()
        {
            ClientList.ListClients.Add(Client);
            Account.CreditPayment = 100;
            Client.ListAccounts.Add(Account);
            FinanceManager.CreditPayment(Guid.NewGuid(), 500);
        }

        [TestMethod]
        public void Convert_ValidTest()
        {
            decimal AmountToConvert = 1000;
            var Euro = new EuroCurrency();
            var ConvertedAmount = Euro.Convert(AmountToConvert);
            Assert.AreEqual(AmountToConvert * 117.7M, ConvertedAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_InvalidTestAccountId()
        {
            decimal AmountToConvert = 1000;
            var Euro = new EuroCurrency();
            var ConvertedAmount = Euro.Convert(-AmountToConvert);
        }

        [TestMethod]
        public void CheckBalance_ValidTest()
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            var Balance = FinanceManager.CheckBalance(Account.Id);
            Assert.AreEqual(0, Balance);
        }

        [DataRow(15)]
        [DataRow(5555)]
        [DataRow(750000)]
        [DataTestMethod]
        public void CheckBalance_InvalidTest(Int32 BalanceToNotExpect)
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            var Balance = FinanceManager.CheckBalance(Account.Id);
            Assert.AreNotEqual(BalanceToNotExpect, (Int32)Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckBalance_InvalidTestAccountId()
        {
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            FinanceManager.CheckBalance(Guid.NewGuid());
        }

        [TestMethod]
        public void AskCredit_ValidTest()
        {
            var Amount = 50000;
            ClientList.ListClients.Add(Client);
            Account.CreditAvailable = true;
            Client.ListAccounts.Add(Account);
            Bank.ListOfCredits.Add(Credit);
            BankList.ListBanks.Add(Bank);
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            FinanceManager.AskCredit(Client.Id, Credit.Id, 5, Amount);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, "Kredit odobren", $"Cestitamo, kredit {Credit.Name} Vam je odobren, zatrazeni iznos {Amount} ce uskoro biti na Vasem racunu.", It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void AskCredit_InvalidTestAmountExceeded()
        {
            var Amount = 500000000;
            ClientList.ListClients.Add(Client);
            Client.ListAccounts.Add(Account);
            Bank.ListOfCredits.Add(Credit);
            BankList.ListBanks.Add(Bank);
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            FinanceManager.AskCredit(Client.Id, Credit.Id, 50, Amount);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, "Kredit odbijen", $"Kredit id {Credit.Id}, nije dostupan u banci {Account.Bank.Name}.", It.IsAny<bool>()), Times.Never);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, "Kredit odbijen", $"Korisnik {Client.Name}, korisnicki id {Client.Id}, jos uvek nema odobrenje za kredit na bilo kom racunu.", It.IsAny<bool>()), Times.Once);

        }


    }
}
