using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.KorisnickiModul.Interfejsi;
using System;

namespace EShoppy.FinansijskiModul.Implementacija
{
    public class Account : IAccount
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public decimal CreditPayment { get; set; }
        public bool CreditAvailable { get; set; }
        public IBank Bank { get; set; }
        public IClient Client { get; private set; }

        // new IAccount(Number, Balance, AskCredit, Bank, Client);
        public Account(string Number, decimal Balance, bool AskCredit, decimal CreditPayment, IBank Bank, IClient Client)
        {
            this.Id = Guid.NewGuid();
            this.Number = Number;
            this.Balance = Balance;
            this.CreditAvailable = AskCredit;
            this.CreditPayment = CreditPayment;
            this.Bank = Bank;
            this.Client = Client;
        }
    }
}
