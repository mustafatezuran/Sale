using System.Threading.Tasks;
using Sale.Api.Model;

namespace Sale.Api.Interfaces
{
    public interface ISaleService
    {
        Task<Model.Sale> GetSale(int saleId);

        Task<Product> GetProduct(int productId);

        Task<Shipping> GetShipping(int shippingId);
    }

}
