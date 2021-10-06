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

namespace EBazaar.UnitTest.TransakcioniModul
{
    [TestClass]
    public class TransactionManagerTests
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
        public void TransactionManager_InvalidClientNotExist()
        {
            var ClientGuid = Guid.NewGuid();
            ClientList.ListClients.Add(Organization);
            Assert.ThrowsException<ArgumentException>(() => TransactionManager.CreateTransaction(ClientGuid, Organization.Id, Guid.NewGuid(), Guid.NewGuid(), 2), $"Klijent sa internim identifikacionim brojem: {ClientGuid}, ne postoji u bazi.");
        }

        [TestMethod]
        public void TransactionManager_InvalidOrganizationNotExist()
        {
            ClientList.ListClients.Add(Client);
            var OrganizationGuid = Guid.NewGuid();
            Assert.ThrowsException<ArgumentException>(() => TransactionManager.CreateTransaction(Client.Id, OrganizationGuid, Guid.NewGuid(), Guid.NewGuid(), 2), $"Klijent sa internim identifikacionim brojem: {OrganizationGuid}, ne postoji u bazi.");
        }

        [TestMethod]
        public void TransactionManager_InvalidOfferNotExist()
        {
            ClientList.ListClients.Add(Client);
            ClientList.ListClients.Add(Organization);
            var OfferGuid = Guid.NewGuid();
            Assert.ThrowsException<KeyNotFoundException>(() => TransactionManager.CreateTransaction(Client.Id, Organization.Id, OfferGuid, Guid.NewGuid(), 2), $"Ne postoji ponuda sa id {OfferGuid} u sistemu.");
        }

        [TestMethod]
        public void TransactionManager_InvalidClientNoAcount()
        {
            ClientList.ListClients.Add(Client);
            ClientList.ListClients.Add(Organization);
            LogisticList.ListOffers.Add(Offer);
            TransactionList.TransactionTypes.Add(TransactionType);
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            Assert.ThrowsException<KeyNotFoundException>(() => TransactionManager.CreateTransaction(Client.Id, Organization.Id, Offer.Id, TransactionType.Id, 5), $"Klijent Marko Markovic, nema dostupnih racuna u sistemu da bi izvrsio placanje.");
            EmailMessageMock.Verify(mock => mock.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void TransactionManager_InvalidOrganizationNoAcount()
        {
            ClientList.ListClients.Clear();
            Client.ListAccounts.Add(Account);
            ClientList.ListClients.Add(Client);
            ClientList.ListClients.Add(Organization);
            Offer.CreatedBy = Organization;
            LogisticList.ListOffers.Add(Offer);
            TransactionList.TransactionTypes.Add(TransactionType);
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            Assert.ThrowsException<KeyNotFoundException>(() => TransactionManager.CreateTransaction(Client.Id, Organization.Id, Offer.Id, TransactionType.Id, 5), $"Klijent {Organization.Name}, nema dostupnih racuna u sistemu da bi naplatio ponudu.");
            EmailMessageMock.Verify(mock => mock.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void TransactionManager_InvalidClientLowBalance()
        {
            ClientList.ListClients.Clear();
            Client.ListAccounts.Add(Account);
            Organization.ListAccounts.Add(OrganizationAccount);
            ClientList.ListClients.Add(Client);
            ClientList.ListClients.Add(Organization);
            LogisticList.ListOffers.Add(Offer);
            TransactionList.TransactionTypes.Add(TransactionType);
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            TransactionManager.CreateTransaction(Client.Id, Organization.Id, Offer.Id, TransactionType.Id, 5);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, "Transakcija neuspesna", $"Nemate dovoljno novca na racunu da se izvrsi transakcija.", It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void TransactionManager_ValidTransaction()
        {
            ClientList.ListClients.Clear();
            Account.Balance = 10000000;
            Client.ListAccounts.Add(Account);
            Organization.ListAccounts.Add(OrganizationAccount);
            ClientList.ListClients.Add(Client);
            ClientList.ListClients.Add(Organization);
            LogisticList.ListOffers.Add(Offer);
            TransactionList.TransactionTypes.Add(TransactionType);
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            TransactionManager.CreateTransaction(Client.Id, Organization.Id, Offer.Id, TransactionType.Id, 5);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, "Transakcija uspesna", It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            EmailMessageMock.Verify(mock => mock.SendEmail(Organization.Email, "Transakcija uspesna", It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
