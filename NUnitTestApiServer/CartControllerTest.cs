using ApiServer.Controllers;
using Entities.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTestApiServer
{
    class CartControllerTest
    {
        private CartController controller;
        private Mock<IPaymentService> paymentServiceMock;
        private Mock<ICartService> cartServiceMock;

        private Mock<IShipmentService> shipmentServiceMock;
        private Mock<ICard> cardMock;
        private Mock<IAddressInfo> addressInfoMock;
        private Mock<ICheckoutData> checkoutDataMock;
        private List<ICartItem> items;

        [SetUp]
        public void Setup()
        {

            cartServiceMock = new Mock<ICartService>();
            paymentServiceMock = new Mock<IPaymentService>();
            shipmentServiceMock = new Mock<IShipmentService>();

            // arrange
            cardMock = new Mock<ICard>();
            addressInfoMock = new Mock<IAddressInfo>();
            checkoutDataMock = new Mock<ICheckoutData>();

            // 
            var cartItemMock = new Mock<ICartItem>();
            cartItemMock.Setup(item => item.Price).Returns(10);

            items = new List<ICartItem>()
            {
              cartItemMock.Object
            };

            cartServiceMock.Setup(c => c.Items()).Returns(items.AsEnumerable());

            controller = new CartController(cartServiceMock.Object, paymentServiceMock.Object, shipmentServiceMock.Object);
        }

        [Test]
        [TestCase(1,  50)] // testing with different set of variables
        [TestCase(20, 50)]
        [MaxTime(1000)]     // test maximum execution time
        public void PrelucrateDataActon_Ok(int param1, decimal param2)
        {
            // act
            var result = controller.PrelucrateDataActon(param1, param2);
            
            // result has StatusCode and NO Object(s)
            var okResult = (IStatusCodeActionResult)result;
            var resutl2 = 10; // result just for comparison sample

            // assert
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.That(resutl2, Is.GreaterThanOrEqualTo(10));

            // only possible if StatusCode has an object attached
            /*var prResult = result as ObjectResult;

            Assert.NotNull(prResult);       // assert
            Assert.True(prResult is OkObjectResult);
            Assert.IsType<TheTypeYouAreExpecting>(prResult.Value);
            Assert.Equal(StatusCodes.Status200OK, prResult.StatusCode); */
        }
        [Test]
        [TearDown]      // test will be executed last after all Tests
                        // Must Not Have Parameters
        public void PrelucrateDataActon_NoContent()
        {
            int param1 = 0; decimal param2 = 10;
            // act
            var result = controller.PrelucrateDataActon(param1, param2);
            var okResult = (IStatusCodeActionResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status204NoContent, okResult.StatusCode);
        }


        [Test]
        public void ShouldReturnCharged()
        {
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), It.IsAny<ICard>())).Returns(true);

            // act
            var result = controller.CheckOut(checkoutDataMock.Object);

            // assert

            // I expect the "Ship()" method to have been called with an "addressInfo" object and "cartItem" list. Called only once.
            // Must "Verify" with the same parameters as the ones used "Once"
            shipmentServiceMock.Verify(s => s.Ship(checkoutDataMock.Object.addressInfo, items.AsEnumerable()), Times.Once());

            // this will work with all IAddressInfo
            shipmentServiceMock.Verify(s => s.Ship(It.IsAny<IAddressInfo>(), items.AsEnumerable()), Times.Once());

            Assert.AreEqual("charged", result);
        }

        [Test]
        public void ShouldReturnNotCharged()
        {
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(false);

            // act
            var result = controller.CheckOut(checkoutDataMock.Object);

            // assert
            shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Never());
            Assert.AreEqual("not charged", result);
        }

    }
}
