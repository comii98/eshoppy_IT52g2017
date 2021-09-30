using EShoppy.FinansijskiModul.Implementacija;
using System;

namespace EShoppy.FinansijskiModul.Interfejsi
{
    public interface ICredit
    {
        /// <summary>
        /// Interni identifikacioni broj kredita
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Naziv kredita
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Maksimalan iznos kredita u RSD
        /// </summary>
        decimal MaxCreditAmount { get; set; }

        /// <summary>
        /// Valuta u kojoj se isplacuje kredit
        /// </summary>
        Currency Currency { get; set; }

        /// <summary>
        /// Mesecna kamatna stopa
        /// </summary>
        double MonthInterestRate { get; set; }

        /// <summary>
        /// Minimalan broj meseci otplate kredita
        /// </summary>
        int MinCreditMonths { get; set; }

        /// <summary>
        /// Maksimalan broj meseci otplate kredita
        /// </summary>
        int MaxCreditMonths { get; set; }

        /// <summary>
        /// Kredit nudi banka
        /// </summary>
        IBank Bank { get; set; }
    }
}
