using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _service;
        private readonly ILogger<CurrencyController> _logger;

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
            _logger.LogInformation("Getting currency ({id}).", currencyId);

            var currency = await _service.GetCurrencyByIdAsync(currencyId);

            if (currency == null)
            {
                _logger.LogWarning("Currency ({currencyId}) was not found.", currencyId);

                return NotFound();
            }

            _logger.LogInformation("Found currency ({currencyId})", currencyId);

            return Ok(currency);
        }
    }
}
