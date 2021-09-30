using System;

namespace EShoppy.ProdajniModul.Interfejsi
{
    public interface IProduct
    {
        /// <summary>
        /// Interni identifikacioni broj proizvoda
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Naziv proizvoda
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Opis proizvoda
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Cena proizvoda
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Proizvod na akciji
        /// </summary>
        bool OnAction { get; set; }
    }
}
