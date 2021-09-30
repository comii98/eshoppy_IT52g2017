using System;

namespace EShoppy.KorisnickiModul.Interfejsi
{
    public interface IOrganization : IClient
    {
        /// <summary>
        /// Poreski identifikacioni broj pravnog lica
        /// </summary>
        Int64 PIB { get; set; }

        /// <summary>
        /// Maticni broj pravnog lica
        /// </summary>
        Int64 MB { get; set; }

        /// <summary>
        /// Sediste kompanije
        /// </summary>
        string HeadOffice { get; set; }
        
        /// <summary>
        /// Datum osnivanja pravnog lica
        /// </summary>
        DateTime FoundedDate { get; set; }

        /// <summary>
        /// Pravni status pravnog lica - Aktivan/Zatvoren
        /// </summary>
        string OperatingStatus { get; set; }
    }
}
