using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exchange.API.Data;
using Exchange.API.Models;
using Exchange.API.Services;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _service;
        private readonly ILogger _logger;

        public CurrencyController(ICurrencyService service, ILogger<CurrencyController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet(Name = "{currencyId}")]
        [ProducesResponseType(typeof(Currency), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Currency>> GetCurrencyByIdAsync([FromQuery] int currencyId)
        {
            var currency = await _service.GetCurrencyByIdAsync(currencyId);

            if (currency == null)
            {
                _logger.LogWarning($"Tried looking for currency {currencyId} but was not found.");

                return NotFound();
            }

            return Ok(currency);
        }
    }
}
