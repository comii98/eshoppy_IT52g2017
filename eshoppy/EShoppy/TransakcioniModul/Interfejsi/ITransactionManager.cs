using System;

namespace EShoppy.TransakcioniModul.Interfejsi
{
    public interface ITransactionManager
    {
        void CreateTransaction(Guid ClientId, Guid OrganizationId, Guid OfferId, Guid TransactionTypeId, int Rating);
    }
}
