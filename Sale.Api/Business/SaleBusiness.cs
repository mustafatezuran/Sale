using System.Threading.Tasks;
using Sale.Api.Entity;
using Sale.Api.Interfaces;

namespace Sale.Api.Business
{
    public class SaleBusiness
    {
        private const string Delivered = "TESLİM EDİLDİ";

        private const string NotDelivered = "TESLİM EDİLMEDİ";

        public ShippingStatus GetShippingStatus(ISaleService saleService, int saleId)
        {
            var response = new ShippingStatus { Sale = null, Product = null, Status = null };

            var saleTask = saleService.GetSale(saleId);
            var shippingTask = saleService.GetShipping(saleId);
            saleTask.Wait(); //sale task and shipping task can work at the same time but product task must get the product id from sale task

            if (saleTask.Result != null)
            {
                var productTask = saleService.GetProduct(saleTask.Result.ProductId);

                Task.WhenAll(productTask, shippingTask);

                response.Sale = saleTask.Result != null
                    ? new ShippingStatusSale
                    {
                        Id = saleTask.Result.Id,
                        Code = saleTask.Result.SaleCode
                    }
                    : null;

                response.Product = productTask.Result;

                response.Status = shippingTask.Result != null
                    ? (shippingTask.Result.Status ? Delivered : NotDelivered)
                    : null;

                return response;
            }

            shippingTask.Wait(); //wait for this task to show only shipping status of sale cause sale response is null

            response.Status = shippingTask.Result != null
                ? (shippingTask.Result.Status ? Delivered : NotDelivered)
                : null;
            
            return response;
        }
    }
}
