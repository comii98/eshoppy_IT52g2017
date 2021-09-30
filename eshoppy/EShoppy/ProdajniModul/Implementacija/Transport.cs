using EShoppy.ProdajniModul.Interfejsi;
using System;
using System.ComponentModel;

namespace EShoppy.ProdajniModul.Implementacija
{
    public class Transport : ITransport
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public double ShippingCoefficient { get; set; }

        public Transport(string Description, double ShippingCoefficient)
        {
            this.Id = Guid.NewGuid();
            this.Description = Description;
            this.ShippingCoefficient = ShippingCoefficient;
        }
    }
}
