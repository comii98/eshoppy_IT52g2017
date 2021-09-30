using System;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class DinarCurrency : Currency
    {
        public override decimal Convert(decimal Amount)
        {
            if (Amount < 0) { throw new ArgumentException(); }
            return Amount;
        }
    }
}
