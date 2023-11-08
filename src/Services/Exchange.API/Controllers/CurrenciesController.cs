using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exchange.API.Data;
using Exchange.API.Models;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : Controller
    {
        private readonly ExchangeAPIContext _context;
        private readonly ILogger _logger;

        public CurrenciesController(ExchangeAPIContext context, ILogger<CurrenciesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "{id}")]
        [ProducesResponseType(typeof(Currency), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Currency>> GetCurrencyByIdAsync([FromQuery] int? id)
        {
            if (id == null || _context.Currency == null)
            {
                return NotFound();
            }

            var currency = await _context.Currency
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currency == null)
            {
                _logger.LogWarning($"Tried looking for currency {id} but was not found.");

                return NotFound();
            }

            return Ok(currency);
        }

        private bool CurrencyExists(int id)
        {
          return (_context.Currency?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
