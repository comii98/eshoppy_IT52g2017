using EShoppy.KorisnickiModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.ProdajniModul.Interfejsi
{
    public interface IOffer
    {
        /// <summary>
        /// Interni identifikacioni broj ponude
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Ponuda kreirana od strane klijenta
        /// </summary>
        IClient CreatedBy { get; set; }

        /// <summary>
        /// Ukupna cena ponude
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Datum i vreme postavljanja ponude
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Lista proizvoda u ponudi
        /// </summary>
        List<IProduct> OfferProducts { get; set; }

        /// <summary>
        /// List transporta izabranih u ponudi
        /// </summary>
        List<ITransport> OfferTransports { get; set; }
    }
}
