using System;
using System.Collections.Generic;
using System.Linq;
using EShoppy.FinansijskiModul.Implementacija;
using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul;
using EShoppy.KorisnickiModul.Implementacija;
using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.PomocniModul.Interfejsi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EBazaar.UnitTest.KorisnickiModul
{
    [TestClass]
    public class ClientManagerTests
    {
        IClientManager ClientManager;
        IClient Client;
        IClient Organization;
        IAccount Account;
        IAccount AccountWithCredit;
        IBank Bank;
        IFinanceManager FinanceManager;
        Mock<IEmailMessage> EmailMessageMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Client = new User("Marko Markovic", "markom@gmail.com", DateTime.Today.AddYears(-20), "+38121000000", "Dunavska 111, 21000 Novi Sad", new List<IAccount>());
            Organization = new Organization("Delta Holding", "office@deltaholding.rs", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "+38163000000", "Aktivno");
            Bank = new Bank("Banca Intesa", "Milentija Popovica, 11100 Novi Beograd", "165");
            Account = new Account("11223344", 0, false, 0, Bank, Organization);
            AccountWithCredit = new Account("44332211", 0, true, 117700, Bank, Organization);
            EmailMessageMock = new Mock<IEmailMessage>();
            FinanceManager = new FinanceManager(EmailMessageMock.Object);
            ClientManager = new ClientManager(EmailMessageMock.Object);
        }

            [TestMethod]
        public void RegisterUser_ValidTest()
        {
            ClientList.ListClients.Clear();
            ClientManager.RegisterUser("Marko Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-20), "Dunavska 111, 21000 Novi Sad");

            var ClientMarkoMarkovic = ClientList.ListClients.Where(c => c.Name == "Marko Markovic").ToList();
            Assert.IsTrue(ClientList.ListClients.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUser_InvalidBirthDateTest()
        {
            ClientManager.RegisterUser("Marko Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-10), "Dunavska 111, 21000 Novi Sad");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterUser_InvalidNameTest()
        {
            ClientManager.RegisterUser(string.Empty, "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-10), "Dunavska 111, 21000 Novi Sad");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterUser_InvalidEmailTest()
        {
            ClientManager.RegisterUser("Marko Markovic", "", "+38121000000", DateTime.Today.AddYears(-10), "Dunavska 111, 21000 Novi Sad");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterUser_InvalidPhoneNumberTest()
        {
            ClientManager.RegisterUser("Marko Markovic", "markom@gmail.com", "", DateTime.Today.AddYears(-10), "Dunavska 111, 21000 Novi Sad");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterUser_InvalidAddressTest()
        {
            ClientManager.RegisterUser("Marko Markovic", "markom@gmail.com", "", DateTime.Today.AddYears(-10), null);

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        public void RegisterOrg_ValidTest()
        {
            ClientList.ListClients.Clear();
            ClientManager.RegisterOrg("Delta Holding", "office@deltaholding.rs", "+38163000000", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "Aktivno");

            Assert.IsTrue(ClientList.ListClients.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterOrg_InvalidNameTest()
        {
            ClientManager.RegisterOrg("", "office@deltaholding.rs", "+38163000000", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "Aktivno");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterOrg_InvalidEmailTest()
        {
            ClientManager.RegisterOrg("Delta Holding", "", "+38163000000", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "Aktivno");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterOrg_InvalidPhoneNumberTest()
        {
            ClientManager.RegisterOrg("Delta Holding", "office@deltaholding.rs", "", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "Aktivno");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterOrg_InvalidPIBTest()
        {
            ClientManager.RegisterOrg("Delta Holding", "office@deltaholding.rs", "+381630001111", -121231323, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "Aktivno");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterOrg_InvalidMBTest()
        {
            ClientManager.RegisterOrg("Delta Holding", "office@deltaholding.rs", "+381630001111", 121231323, 1234567899898, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "Aktivno");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterOrg_InvalidOperatingStatusTest()
        {
            ClientManager.RegisterOrg("Delta Holding", "office@deltaholding.rs", "+381630001111", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "");

            Assert.IsTrue(ClientList.ListClients.Count == 0);
        }

        [TestMethod]
        public void ChangeUserAccount_ValidTest()
        {
            ClientList.ListClients.Clear();
            ClientManager.RegisterOrg("NIS Srbija", "office@nissrbija.rs", "+381630001111", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-55), "Aktivno");

            var OrganizationToUpdate = ClientList.ListClients.FirstOrDefault(c => c.Name == "NIS Srbija");

            Assert.IsTrue(OrganizationToUpdate.Email.Equals("office@nissrbija.rs"));
            Assert.IsTrue(OrganizationToUpdate.ListAccounts.Count == 0);
            ClientManager.ChangeOrgAccount(OrganizationToUpdate as Organization, new List<IAccount> { Account });
            Assert.IsTrue(OrganizationToUpdate.ListAccounts.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangeUserAccount_InvalidTest()
        {
            ClientManager.RegisterOrg("NIS Srbija Invalid", "office@nissrbija.rs", "+381630001111", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-55), "Aktivno");

            var OrganizationToUpdate = ClientList.ListClients.FirstOrDefault(c => c.Name == "NIS Srbija Invalid");

            Assert.IsTrue(OrganizationToUpdate.Email.Equals("office@nissrbija.rs"));
            Assert.IsTrue(OrganizationToUpdate.ListAccounts.Count == 0);
            ClientManager.ChangeOrgAccount(OrganizationToUpdate as Organization, null);
        }

        [TestMethod]
        public void GetClientById_ValidTest()
        {
            IClientManager clientManager = new ClientManager();

            ClientList.ListClients.Clear();
            ClientManager.RegisterUser("Marko Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-20), "Dunavska 111, 21000 Novi Sad");

            Guid Id = ClientList.ListClients[0].Id;
            IClient Client = ClientManager.GetClientByID(Id);

            Assert.AreEqual(Client.Id, Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetClientById_InvalidTest()
        {
            IClientManager clientManager = new ClientManager();

            ClientList.ListClients.Clear();
            ClientManager.RegisterUser("Marko Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-20), "Dunavska 111, 21000 Novi Sad");

            Guid Id = ClientList.ListClients[0].Id;
            ClientList.ListClients.Clear();
            ClientManager.GetClientByID(Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFunds_InvalidArgumentsTest()
        {
            ClientManager.AddFunds(null, Guid.NewGuid(), 123, null);
        }

        [TestMethod]
        public void AddFunds_ValidCheckUpdateBalance()
        {
            decimal Amount = 1000M;
            ClientList.ListClients.Clear();
            ClientManager.RegisterUser("Marko M Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-20), "Dunavska 111, 21000 Novi Sad");

            var UserToUpdate = ClientList.ListClients.FirstOrDefault(c => c.Name == "Marko M Markovic") as User;

            Assert.IsTrue(UserToUpdate.ListAccounts.Count == 0);
            var AccountsForUser = new List<IAccount> { Account };
            ClientManager.ChangeUserAccount(UserToUpdate, AccountsForUser);
            Assert.IsTrue(UserToUpdate.ListAccounts.Count == 1);
            decimal OldBalance = UserToUpdate.ListAccounts.FirstOrDefault().Balance;
            var Euro = new EuroCurrency();
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            ClientManager.AddFunds(UserToUpdate, Account.Id, Amount, Euro);

            Assert.AreEqual(OldBalance + Euro.Convert(Amount), UserToUpdate.ListAccounts.FirstOrDefault().Balance);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void AddFunds_ValidCheckCreditPayment()
        {
            decimal Amount = 1000M;
            ClientList.ListClients.Clear();
            ClientManager.RegisterUser("Marko M Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-20), "Dunavska 111, 21000 Novi Sad");

            var UserToUpdate = ClientList.ListClients.FirstOrDefault(c => c.Name == "Marko M Markovic") as User;

            Assert.IsTrue(UserToUpdate.ListAccounts.Count == 0);
            var AccountsForUser = new List<IAccount> { AccountWithCredit };
            ClientManager.ChangeUserAccount(UserToUpdate, AccountsForUser);
            Assert.IsTrue(UserToUpdate.ListAccounts.Count == 1);
            decimal OldBalance = UserToUpdate.ListAccounts.FirstOrDefault().Balance;
            var Euro = new EuroCurrency();
            EmailMessageMock.Setup(_ => _.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
            ClientManager.AddFunds(UserToUpdate, AccountWithCredit.Id, Amount, Euro);

            Assert.AreEqual(0, UserToUpdate.ListAccounts.FirstOrDefault().CreditPayment);
            EmailMessageMock.Verify(mock => mock.SendEmail(Client.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddFunds_InvalidCheckAccountNotExist()
        {
            decimal Amount = 1000M;
            ClientManager.RegisterUser("Marko X Markovic", "markom@gmail.com", "+38121000000", DateTime.Today.AddYears(-20), "Dunavska 111, 21000 Novi Sad");

            var UserToUpdate = ClientList.ListClients.FirstOrDefault(c => c.Name == "Marko X Markovic") as User;

            Assert.IsTrue(UserToUpdate.ListAccounts.Count == 0);
            var AccountsForUser = new List<IAccount> { AccountWithCredit };
            ClientManager.ChangeUserAccount(UserToUpdate, AccountsForUser);
            Assert.IsTrue(UserToUpdate.ListAccounts.Count == 1);
            decimal OldBalance = UserToUpdate.ListAccounts.FirstOrDefault().Balance;
            var Euro = new EuroCurrency();
            ClientManager.AddFunds(UserToUpdate, Guid.NewGuid(), Amount, Euro);
        }
    }
}
