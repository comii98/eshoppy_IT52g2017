using System;

namespace EShoppy.KorisnickiModul.Interfejsi
{
    /// <summary>
    /// Interfejs za fizicke korisnika sistema
    /// </summary>
    public interface IUser : IClient
    {   
        /// <summary>
        /// Datum rodjenja
        /// </summary>
        DateTime BirthDate { get; set; }

        /// <summary>
        /// Adresa fizickog lica
        /// </summary>
        string Address { get; set; }
    }
}
