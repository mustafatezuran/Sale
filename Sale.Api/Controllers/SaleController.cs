using Microsoft.AspNetCore.Mvc;
using Sale.Api.Business;
using Sale.Api.Interfaces;
using Sale.Api.Entity;

namespace Sale.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService) => _saleService = saleService;

        // GET sale/{saleId}/shipping
        [HttpGet("{saleId}/shipping")]
        public ActionResult<ShippingStatus> Get(int saleId) => Ok(new SaleBusiness().GetShippingStatus(_saleService, saleId));
    }
}