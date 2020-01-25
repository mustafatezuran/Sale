using Sale.Api.Model;

namespace Sale.Api.Entity
{
    public class ShippingStatus
    {
        public string Status { get; set; }
        public ShippingStatusSale Sale { get; set; }
        public Product Product { get; set; }
    }
}
