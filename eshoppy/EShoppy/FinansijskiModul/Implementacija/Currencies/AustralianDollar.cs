using System;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class AustralianDollar : Currency
    {
        public override decimal Convert(decimal Amount)
        {
            if (Amount < 0) { throw new ArgumentException(); }
            return Amount * 99.5M;
        }
    }
}
