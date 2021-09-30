using EShoppy.FinansijskiModul.Implementacija;
using EShoppy.KorisnickiModul.Interfejsi;
using System;

namespace EShoppy.FinansijskiModul.Interfejsi
{
    /// <summary>
    /// Interfejs finansijskog modula
    /// </summary>
    public interface IFinanceManager
    {
        void CreateAccount(string Number, bool AskCredit, IBank Bank, IClient Client);

        void CreateBank(string Name, string HeadOffice, string AccountPrefix);

        void CreateCredit(string Name, decimal MaxCreditAmount, double MonthInterestRate, int MinCreditMonths, int MaxCreditMonths, Currency Currency, IBank Bank);

        IAccount GetAccountByID(Guid AccountId);

        void AskCredit(Guid ClientId, Guid CreditId, int PaymentPeriod, decimal Amount);

        void AccountPayment(Guid AccountId, decimal Amount);

        void CreditPayment(Guid AccountId, decimal Amount);

        decimal Convert(Currency Currency, decimal Amount);

        decimal CheckBalance(Guid AccountId);
    }
}
