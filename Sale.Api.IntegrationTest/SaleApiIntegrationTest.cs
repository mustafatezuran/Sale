using Newtonsoft.Json;
using Sale.Api.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sale.Api.IntegrationTest
{
    public class SaleApiIntegrationTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public SaleApiIntegrationTest(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Theory]
        [InlineData(3)]
        public async Task Get_Should_Return_OK_With_Expected_Shipping_Status(int id)
        {
            var expectedStatusCode = HttpStatusCode.OK;

            var responseGet = await _client.GetAsync($"/sale/{ id }/shipping");
            var actualStatusCode = responseGet.StatusCode;

            var actualGetResult = await responseGet.Content.ReadAsStringAsync();

            var getResult = JsonConvert.DeserializeObject<ShippingStatus>(actualGetResult);

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal("0c784421-3cbb-4566-ab72-06628413758b", getResult.Sale.Code);
            Assert.Equal("Generic Concrete Ball", getResult.Product.Name);
            Assert.Equal("TESLÝM EDÝLMEDÝ", getResult.Status);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public async Task Get_Should_Return_OK_With_Null_Response(int saleId)
        {
            var expectedStatusCode = HttpStatusCode.OK;

            var responseGet = await _client.GetAsync($"/sale/{saleId}/shipping");
            var actualStatusCode = responseGet.StatusCode;

            var actualGetResult = await responseGet.Content.ReadAsStringAsync();

            var getResult = JsonConvert.DeserializeObject<ShippingStatus>(actualGetResult);
            var nullShippingStatus = new ShippingStatus { Sale = null, Product = null, Status = null };
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(JsonConvert.SerializeObject(nullShippingStatus), JsonConvert.SerializeObject(getResult));
        }
    }
}
