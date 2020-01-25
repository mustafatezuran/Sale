using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Sale.Api.Controllers;
using Sale.Api.Entity;
using Sale.Api.Interfaces;
using Sale.Api.Model;
using System.Threading.Tasks;
using Xunit;

namespace Sale.Api.Test
{
    public class SaleApiTest
    {
        [Theory, AutoMoqData]
        public void GetSale_Should_Return_As_Expected(Mock<ISaleService> customerServiceMock, int id, Model.Sale expected)
        {
            var sut = new SaleController(customerServiceMock.Object);
            customerServiceMock.Setup(m => m.GetSale(id)).Returns(Task.FromResult(expected));

            var result = sut.Get(id);

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actual = apiOkResult.Value.Should().BeAssignableTo<ShippingStatus>().Subject;

            var actualSale = new Model.Sale { Id = actual.Sale.Id, SaleCode = actual.Sale.Code };
            expected.ProductId = 0; //customer controller returns ShippingStatusSale which has not ProductId

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actualSale));
        }

        [Theory, AutoMoqData]
        public void GetProduct_Should_Return_As_Expected(Mock<ISaleService> customerServiceMock, int id, Model.Sale expectedSale, Product expectedProduct)
        {
            var sut = new SaleController(customerServiceMock.Object);
            customerServiceMock.Setup(m => m.GetSale(id)).Returns(Task.FromResult(expectedSale));
            customerServiceMock.Setup(m => m.GetProduct(expectedSale.ProductId)).Returns(Task.FromResult(expectedProduct));

            var result = sut.Get(id);

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actual = apiOkResult.Value.Should().BeAssignableTo<ShippingStatus>().Subject;
            
            Assert.Equal(JsonConvert.SerializeObject(expectedProduct), JsonConvert.SerializeObject(actual.Product));
        }

        [Theory, AutoMoqData]
        public void GetShipping_Should_Return_As_Expected(Mock<ISaleService> customerServiceMock, int id, Shipping expected)
        {
            var sut = new SaleController(customerServiceMock.Object);
            customerServiceMock.Setup(m => m.GetShipping(id)).Returns(Task.FromResult(expected));

            var result = sut.Get(id);

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actual = apiOkResult.Value.Should().BeAssignableTo<ShippingStatus>().Subject;

            if (expected == null)
            {
                Assert.Null(actual.Status);
            }
            else if (expected.Status)
            {
                Assert.Equal("TESLÝM EDÝLDÝ", actual.Status);
            }
            else
            {
                Assert.Equal("TESLÝM EDÝLMEDÝ", actual.Status);
            }
        }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
#pragma warning disable CS0618 // Type or member is obsolete
            : base(new Fixture().Customize(new AutoMoqCustomization()))
#pragma warning restore CS0618 // Type or member is obsolete
        {
        }
    }
}
