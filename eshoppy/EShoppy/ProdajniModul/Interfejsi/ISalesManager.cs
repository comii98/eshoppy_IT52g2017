using EShoppy.KorisnickiModul.Interfejsi;
using System;
using System.Collections.Generic;

namespace EShoppy.ProdajniModul.Interfejsi
{
    public interface ISalesManager
    {
        void CreateProduct(string Name, string Description, decimal Price, bool OnAction);

        void CreateOffer(IClient CreatedBy, List<IProduct> Products, List<ITransport> Transports);

        List<IOffer> GetOffersByTransportId(Guid TransportId);

        List<IOffer> GetOffersByProduct(Guid ProductId);

        IOffer GetLowestOffer(List<IOffer> Offers);

        decimal GetTransportCost(IOffer Offer, ITransport Transport);

        void UpdateOffer(IOffer Offer, decimal? Price, List<IProduct> Products, List<ITransport> Transports, IClientManager Client);

    }
}
