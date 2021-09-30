using EShoppy.TransakcioniModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.TransakcioniModul.Implementacija
{
    public class TransactionType : ITransactionType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<double> TransactionPieces { get; set; }

        public TransactionType(string Name, string Description, List<double> TransactionPieces)
        {
            this.Name = Name;
            this.Description = Description;
            this.TransactionPieces = TransactionPieces;
        }
    }
}
