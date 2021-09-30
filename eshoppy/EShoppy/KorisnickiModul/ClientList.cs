using EShoppy.KorisnickiModul.Interfejsi;
using System.Collections.Generic;

namespace EShoppy.KorisnickiModul
{
    public static class ClientList
    {
        /// <summary>
        /// Listu svih klijenata sistema i koristiće se za pretrage korisnika
        /// </summary>
        public static List<IClient> ListClients { get; private set; } = new List<IClient>();
    }
}
