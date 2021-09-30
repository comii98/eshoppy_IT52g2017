using EShoppy.FinansijskiModul.Implementacija;
using EShoppy.FinansijskiModul.Interfejsi;
using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.KorisnickiModul.Interfejsi
{
    public interface IClientManager
    {

        void RegisterUser(string Name, string Email, string PhoneNumber, DateTime BirthDate, string Address);

        void RegisterOrg(string Name, string Email, string PhoneNumber, Int64 PIB, Int64 MB, string HeadOffice, DateTime FoundedDate, string OperatingStatus);

        void ChangeUserAccount(IUser User, List<IAccount> Accounts);

        void ChangeOrgAccount(IOrganization Organization, List<IAccount> Accounts);

        List<ITransaction> SearchHistory(IClient Client, DateTime? Date, int? TransactionType, int? Rating);

        IClient GetClientByID(Guid ClientId);

        void AddFunds(IClient Client, Guid AccountId, decimal Amount, Currency Currency);
    }
}
