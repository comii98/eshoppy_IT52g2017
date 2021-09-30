using System;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class LeiCurrency : Currency
    {
        public override decimal Convert(decimal Amount)
        {
            if (Amount < 0) { throw new ArgumentException(); }
            return Amount * 23.74M;
        }
    }
}
