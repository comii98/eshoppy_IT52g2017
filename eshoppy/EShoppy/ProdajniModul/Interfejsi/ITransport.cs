using System;

namespace EShoppy.ProdajniModul.Interfejsi
{
    public interface ITransport
    {
        /// <summary>
        /// Interni identifikacioni broj transporta
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Opis transporta
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Nacin slanja, odredjen koeficijentom
        /// </summary>
        double ShippingCoefficient{ get; set; }
    }
}
