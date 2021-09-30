using EShoppy.KorisnickiModul.Interfejsi;
using EShoppy.TransakcioniModul.Interfejsi;
using System;

namespace EShoppy.TransakcioniModul.Implementacija
{
    public class Transaction : ITransaction
    {
        public Guid Id { get ; set ; }
        public IClient Client { get ; set ; }
        public decimal Price { get ; set ; }
        public double Discount { get ; set ; }
        public DateTime CreatedAt { get ; set ; }
        public int Rating { get ; set ; }
    }
}
