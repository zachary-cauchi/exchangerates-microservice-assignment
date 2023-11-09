using Exchange.API.Models;
using Exchange.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountBalanceController : Controller
    {
        private readonly IAccountBalanceService _service;
        private readonly ILogger<AccountBalanceController> _logger;
        private readonly IMediator _mediator;

        public AccountBalanceController(IAccountBalanceService accountBalanceService, ILogger<AccountBalanceController> logger, IMediator mediator)
        {
            _service = accountBalanceService ?? throw new ArgumentNullException(nameof(accountBalanceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator;
        }

        [HttpGet(Name = "{accountBalanceId}")]
        [ProducesResponseType(typeof(AccountBalance), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountBalance>> GetAccountBalanceByIdAsync([FromQuery] int accountBalanceId)
        {
            var accountBalance = await _service.GetAccountBalanceByIdAsync(accountBalanceId);

            if (accountBalance == null)
            {
                return NotFound();
            }

            return Ok(accountBalance);
        }

        [HttpPost(Name = "/exchange")]
        [ProducesResponseType(typeof(AccountBalance), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PastTransaction>> ExchangeCurrenciesBetweenUserAccountsAsync(int srcAccountId, int destAccountId, decimal srcAmount)
        {
            //AccountBalance accountBalance;

            try
            {
                CreateCurrencyExchangeCommand createCurrencyExchangeCommand = new CreateCurrencyExchangeCommand(fromAccountBalanceId: srcAccountId, toAccountBalanceId: destAccountId, debitAmount: srcAmount, timeOfRate: DateTime.UtcNow, exchangeRate: 2);
                var result = await _mediator.Send(createCurrencyExchangeCommand);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
