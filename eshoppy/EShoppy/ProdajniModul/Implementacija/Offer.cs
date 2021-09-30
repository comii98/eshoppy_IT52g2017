using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.ProdajniModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.ProdajniModul.Implementacija
{
    public class Offer : IOffer
    {
        public Offer(IClient CreatedBy, List<IProduct> Products, List<ITransport> Transports, decimal Price, DateTime CreatedAt)
        {
            this.Id = Guid.NewGuid();
            this.CreatedBy = CreatedBy;
            this.OfferProducts = Products;
            this.OfferTransports = Transports;
            this.Price = Price;
            this.CreatedAt = CreatedAt;
        }

        public Guid Id { get; set; }
        public IClient CreatedBy { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<IProduct> OfferProducts { get; set; }
        public List<ITransport> OfferTransports { get; set; }
    }
}
