using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.ProdajniModul.Interfejsi;
using EShoppy.KorisnickiModul.Implementacija;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EShoppy.ProdajniModul.Implementacija
{
    public class SalesManager : ISalesManager
    {
        public void CreateOffer(IClient CreatedBy, List<IProduct> Products, List<ITransport> Transports)
        {
            if (CreatedBy == null) { throw new ArgumentNullException(); }
            if (Products == null) { throw new ArgumentNullException(); }
            if (Products.Count < 0) { throw new ArgumentException(); };
            if (Transports == null) { throw new ArgumentNullException(); }
            if (Transports.Count < 0) { throw new ArgumentException(); };

            IClientManager ClientManager = new ClientManager();
            IClient ClientToOffer = ClientManager.GetClientByID(CreatedBy.Id);

            decimal FullPrice = Products.Sum(p => p.Price);
            IOffer OfferForClient = new Offer(CreatedBy, Products, Transports, FullPrice, DateTime.Now);

            LogisticList.ListOffers.Add(OfferForClient);
        }

        public void CreateProduct(string Name, string Description, decimal Price, bool OnAction)
        {
            if (string.IsNullOrEmpty(Name)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(Description)) { throw new ArgumentNullException(); }
            if (Price < 0) { throw new ArgumentException(); };

            IProduct Product = new Product(Name, Description, Price, OnAction);

            LogisticList.ListProducts.Add(Product);
        }

        public IOffer GetLowestOffer(List<IOffer> Offers)
        {
            if (Offers == null) { throw new ArgumentNullException(); }
            if (Offers.Count < 0) { throw new ArgumentException(); };

            IOffer MinOffer = Offers.OrderBy(o => o.Price).First();

            return MinOffer;
        }



        public List<IOffer> GetOffersByProduct(Guid ProductId)
        {
            if (ProductId == null) { throw new ArgumentNullException(); }

            var OffersWithProduct = LogisticList.ListOffers.Where(o => o.OfferProducts.FirstOrDefault(p => p.Id == ProductId) != null).ToList();
            return OffersWithProduct;
        }

        public List<IOffer> GetOffersByTransportId(Guid TransportId)
        {
            if (TransportId == null) { throw new ArgumentNullException(); }

            var OffersWithTransport = LogisticList.ListOffers.Where(o => o.OfferTransports.FirstOrDefault(p => p.Id == TransportId) != null).ToList();
            return OffersWithTransport;
        }

        public decimal GetTransportCost(IOffer Offer, ITransport Transport)
        {
            if (Offer == null) { throw new ArgumentNullException(); }
            if (Transport == null) { throw new ArgumentNullException(); }

            var TransportCost = Offer.Price * (decimal)Transport.ShippingCoefficient;
            return TransportCost;
        }

        public void UpdateOffer(IOffer Offer, decimal? Price, List<IProduct> Products, List<ITransport> Transports, IClientManager Client)
        {
            if (Offer == null) { throw new ArgumentNullException(); }

            if (Price != null)
            {
                Offer.Price = Price.Value;
            }

            if (Products != null)
            {
                Offer.OfferProducts = Products;
            }

            if (Transports != null)
            {
                Offer.OfferTransports = Transports;
            }
        }
    }
}
