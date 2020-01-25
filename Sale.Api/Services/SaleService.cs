using System.Net.Http;
using System.Threading.Tasks;
using Sale.Api.Interfaces;
using Sale.Api.Model;

namespace Sale.Api.Services
{
    public class SaleService : ISaleService
    {
        private HttpClient Client { get; }

        public SaleService(HttpClient client)
        {
            Client = client;
        }

        public async Task<Product> GetProduct(int productId) => await GetAsync<Product>($"products/{productId}");

        public async Task<Model.Sale> GetSale(int saleId) => await GetAsync<Model.Sale>($"sales/{saleId}");

        public async Task<Shipping> GetShipping(int shippingId) => await GetAsync<Shipping>($"shipping/{shippingId}");

        private async Task<T> GetAsync<T>(string url) //where T: new()
        {
            var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            //log response.ReasonPhrase or throw new Exception(response.ReasonPhrase)
            //return new T();

            return default(T);
        }
    }
}