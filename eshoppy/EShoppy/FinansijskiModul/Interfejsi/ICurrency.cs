using System;

namespace EShoppy.FinansijskiModul.Interfejsi
{
    public interface ICurrency
    {
        /// <summary>
        /// Interni identifikacioni broj valute
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Naziv valute
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Skraceni kod valute
        /// </summary>
        string Code { get; set; }
    }
}
