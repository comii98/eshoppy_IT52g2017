using EShoppy.ProdajniModul.Interfejsi;
using System;

namespace EShoppy.ProdajniModul.Implementacija
{
    public class Product : IProduct
    {
        public Product(string name, string description, decimal price, bool onAction)
        {
            Name = name;
            Description = description;
            Price = price;
            OnAction = onAction;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool OnAction { get; set; }
    }
}
