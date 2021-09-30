using EShoppy.FinansijskiModul.Interfejsi;
using System;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public abstract class Currency : ICurrency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public abstract decimal Convert(decimal Amount);
    }
}
