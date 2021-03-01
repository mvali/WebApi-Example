using Entities.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // for testing purposes - in Test Project
        [HttpPost]
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

        [HttpGet]
        [Route("/api/[controller]/dataaction")]
        public IActionResult PrelucrateDataActon(int param1, decimal param2)
        {
            if (param1 > 0)
                return Ok();

            return NoContent();
        }

        [HttpGet]
        [Route("/api/[controller]/dataint")]
        public int PrelucrateDataInt(int param1, decimal param2)
        {
            return param1 > 10 ? 1 : 1;
        }


    }
    public class ShippmentService : IShipmentService
    {
        public void Ship(IAddressInfo info, IEnumerable<ICartItem> items)
        {
            // I ship for test method
        }
    }
}
