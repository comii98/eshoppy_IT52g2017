using EShoppy.FinansijskiModul.Interfejsi;
using System;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class Credit : ICredit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal MaxCreditAmount { get; set; }
        public Currency Currency { get; set; }
        public double MonthInterestRate { get; set; }
        public int MinCreditMonths { get; set; }
        public int MaxCreditMonths { get; set; }
        public IBank Bank { get; set; }

        public Credit(string Name, decimal MaxCreditAmount, Currency Currency, double MonthInterestRate, int MinCreditMonths, int MaxCreditMonths, IBank Bank)
        {
            Id = Guid.NewGuid();
            this.Name = Name;
            this.MaxCreditAmount = MaxCreditAmount;
            this.Currency = Currency;
            this.MonthInterestRate = MonthInterestRate;
            this.MinCreditMonths = MinCreditMonths;
            this.MaxCreditMonths = MaxCreditMonths;
            this.Bank = Bank;
        }
    }
}
