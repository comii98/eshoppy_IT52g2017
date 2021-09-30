using System;
using System.Collections.Generic;

namespace EShoppy.FinansijskiModul.Interfejsi
{
    public interface IBank
    {
        /// <summary>
        /// Interni identifikacioni broj banke
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Naziv banke
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Sediste banke
        /// </summary>
        string HeadOffice { get; set; }

        /// <summary>
        /// Rezervisani prefiks banke za racune
        /// </summary>
        string AccountPrefix { get; set; }

        /// <summary>
        /// Lista kredita koje banka ima u ponudi
        /// </summary>
        List<ICredit> ListOfCredits { get; set; }

        /// <summary>
        /// List registrovanih racuna banke
        /// </summary>
        List<IAccount> ListOfAccounts { get; set; }

    }
}
