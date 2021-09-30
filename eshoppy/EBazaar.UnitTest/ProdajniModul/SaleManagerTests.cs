using System;
using System.Collections.Generic;
using System.Linq;
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

namespace EBazaar.UnitTest.ProdajniModul
{
    [TestClass]
    public class SaleManagerTests
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
        ISalesManager SalesManager;

        [TestInitialize]
        public void TestInitialize()
        {
            Client = new User("Marko Markovic", "markom@gmail.com", DateTime.Today.AddYears(-20), "+38121000000", "Dunavska 111, 21000 Novi Sad", new List<IAccount>());
            Organization = new Organization("Delta Holding", "office@deltaholding.rs", 1110001111, 12345678, "Petra Petrovica bb, 11000 Beograd", DateTime.Now.AddYears(-30), "+38163000000", "Aktivno");
            Bank = new Bank("Banca Intesa", "Milentija Popovica, 11100 Novi Beograd", "165");
            Account = new Account("11223344", 0, false, 0, Bank, Client);
            OrganizationAccount = new Account("00110011", 0, false, 0, Bank, Organization);
            Product = new Product("Proizvod 1", "Opis proizvoda 1", 100000, false);
            Transport = new Transport("Transport 1", 1);
            Offer = new Offer(Organization, new List<IProduct> { Product }, new List<ITransport> { Transport }, 200000, DateTime.Now.AddMonths(-7));
            TransactionType = new TransactionType("Placanje u celosti", "Placanje u celosti ponude sa uracunatim popustima", new List<double> { 1 });
            //AccountWithCredit = new Account("44332211", 0, true, 117700, Bank, Organization);
            SalesManager = new SalesManager();
            EmailMessageMock = new Mock<IEmailMessage>();
            TransactionManager = new TransactionManager(EmailMessageMock.Object);
            FinanceManager = new FinanceManager(EmailMessageMock.Object);
            ClientManager = new ClientManager(EmailMessageMock.Object);
        }

        [TestMethod]
        public void CreateProduct_ValidTest()
        {
            int ProductsInSystem = LogisticList.ListProducts.Count;
            SalesManager.CreateProduct("Product", "Product Desc", 0, false);
            Assert.AreEqual(ProductsInSystem, LogisticList.ListProducts.Count - 1);
            Assert.AreEqual(0, LogisticList.ListProducts.First().Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateProduct_InvalidNameEmpty()
        {
            SalesManager.CreateProduct("", "Product Desc", 0, false);
        }

        [TestMethod]
        public void CreateOffer_ValidTest()
        {
            LogisticList.ListOffers.Clear();
            ClientList.ListClients.Add(Client);
            int OffersInSystem = LogisticList.ListOffers.Count;
            SalesManager.CreateOffer(Client, new List<IProduct> { Product }, new List<ITransport> { Transport });
            Assert.AreEqual(OffersInSystem, LogisticList.ListOffers.Count - 1);
            Assert.AreEqual(100000, LogisticList.ListOffers.First().Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOffer_InvalidNullValues()
        {
            SalesManager.CreateOffer(null, null, null);
        }

        [TestMethod]
        public void GetOffersByTransportId_ValidTest()
        {
            var TransportList = new List<ITransport> { Transport };
            ClientList.ListClients.Add(Client);
            int OffersInSystem = LogisticList.ListOffers.Count;

            SalesManager.CreateOffer(Client, new List<IProduct>(), TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>(), TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>(), TransportList);

            var OffersForTransportId = SalesManager.GetOffersByTransportId(Transport.Id);

            Assert.AreEqual(OffersForTransportId.Count, 3);
        }

        [TestMethod]
        public void GetOffersByTransportId_InvalidTest()
        {
            var TransportList = new List<ITransport> { Transport };
            ClientList.ListClients.Add(Client);
            int OffersInSystem = LogisticList.ListOffers.Count;

            SalesManager.CreateOffer(Client, new List<IProduct>(), TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>(), TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>(), TransportList);

            var OffersForTransportId = SalesManager.GetOffersByTransportId(Guid.NewGuid());

            Assert.AreEqual(OffersForTransportId.Count, 0);
        }

        [TestMethod]
        public void GetLowestOffer_ValidTest()
        {
            var TransportList = new List<ITransport> { Transport };
            ClientList.ListClients.Add(Client);

            IProduct ProductLowestPrice = Product;
            ProductLowestPrice.Price -= 1;

            IProduct ProductHighestPrice = Product;
            ProductHighestPrice.Price += 1;

            LogisticList.ListOffers.Clear();

            SalesManager.CreateOffer(Client, new List<IProduct>() { Product }, TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>() { Product, ProductHighestPrice}, TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>() { Product, ProductLowestPrice}, TransportList);

            IOffer LowestPriceOffer = SalesManager.GetLowestOffer(LogisticList.ListOffers);

            Assert.AreEqual(Product.Price, LowestPriceOffer.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetLowestOffer_InvalidTest()
        {
            var TransportList = new List<ITransport> { Transport };
            ClientList.ListClients.Add(Client);

            IProduct ProductLowestPrice = Product;
            ProductLowestPrice.Price -= 1;

            IProduct ProductHighestPrice = Product;
            ProductHighestPrice.Price += 1;

            LogisticList.ListOffers.Clear();

            SalesManager.CreateOffer(Client, new List<IProduct>() { Product }, TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>() { Product, ProductHighestPrice }, TransportList);
            SalesManager.CreateOffer(Client, new List<IProduct>() { Product, ProductLowestPrice }, TransportList);

            IOffer LowestPriceOffer = SalesManager.GetLowestOffer(null);
        }

        [TestMethod]
        public void GetTransportCost_ValidTest()
        {
            decimal FullPrice = SalesManager.GetTransportCost(Offer, Transport);

            Assert.AreEqual(FullPrice, Offer.Price * (decimal)Transport.ShippingCoefficient);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTransportCost_InvalidTest()
        {
            var TransportList = new List<ITransport> { Transport };
            var TransportTest = new Transport(Transport.Description, Transport.ShippingCoefficient);
            ClientList.ListClients.Add(Client);
            LogisticList.ListOffers.Clear();

            SalesManager.CreateOffer(Client, new List<IProduct>() { Product }, TransportList);

            decimal FullPrice = SalesManager.GetTransportCost(Offer, null);
        }
    }
}
