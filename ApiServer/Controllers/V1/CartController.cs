using Entities.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers.V1
{
    // This is Version2 of CartController build separate for not breaking the logic of V1 by accident
    // This is Original Version1 of CartController

    //[ApiExplorerSettings(IgnoreApi = true, GroupName = nameof(CartController))]
    [Route("api/v1/[controller]/")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiExplorerSettings(IgnoreApi = true)] // ignore this method from api documentation
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult PrelucrateDataActon(int param1, decimal param2)
        {
            if (param1 > 0)
                return Ok();

            return NoContent();
        }
    }
}
