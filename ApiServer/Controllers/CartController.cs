using Entities.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    // for testing purposes - in Test Project, Versioning test

    //[ApiExplorerSettings(IgnoreApi = true, GroupName = nameof(CartController))]
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)] // is a good practice to publish in header deprecated version
    [ApiVersion("2.0")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        private readonly IShipmentService _shipmentService;

        public CartController(ICartService cartService, IPaymentService paymentService, IShipmentService shipmentService)
        {
            _cartService = cartService;
            _paymentService = paymentService;
            _shipmentService = shipmentService;
        }

        
        [HttpGet("{productId}")]   // api/cart/1
        [ApiVersion("1.0", Deprecated = true)] // is a good practice to publish in header deprecated version
        public IActionResult GetProductV1([FromRoute]int productId)
        {
            var product = new ProductResponseV1
            {
                Id = productId,
                Name = "product name for api V1"
            };

            return Ok(product);
        }
        [HttpGet("{productId}")]
        //[ApiVersion("2.0")]
        [MapToApiVersion("2.0")]
        public IActionResult GetProductV2([FromRoute] int productId)
        {
            var product = new ProductResponseV2
            {
                Id = productId,
                Username = "product username for api V2"
            };

            return Ok(product);
        }

        [HttpGet]
        [Route("/api/[controller]/dataaction")]
        [MapToApiVersion("1.0")]
        public IActionResult PrelucrateDataActon(int param1, decimal param2)
        {
            if (param1 > 0)
                return Ok();

            return NoContent();
        }

        [HttpGet]
        [Route("/api/[controller]/dataint")]
        [MapToApiVersion("2.0")]
        public int PrelucrateDataInt(int param1, decimal param2)
        {
            return param1 > 10 ? 1 : 1;
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)] // ignore this method from api documentation
        public string CheckOut(ICheckoutData ckdata)
        {
            var result = _paymentService.Charge(_cartService.Total(), ckdata.card);
            if (result)
            {
                _shipmentService.Ship(ckdata.addressInfo, _cartService.Items());
                return "charged";
            }
            else
            {
                return "not charged";
            }
        }


    }
    public class ProductResponseV1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProductResponseV2
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
    public class ShippmentService : IShipmentService
    {
        public void Ship(IAddressInfo info, IEnumerable<ICartItem> items)
        {
            // I ship for test method
        }
    }
}
