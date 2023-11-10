using Exchange.API.Models;
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

            _logger.LogInformation("Getting account balance ({id}).", accountBalanceId);

            var accountBalance = await _service.GetAccountBalanceByIdAsync(accountBalanceId);

            if (accountBalance == null)
            {
                _logger.LogWarning("Account balance ({id}) not found.", accountBalanceId);
                return NotFound();
            }

            _logger.LogInformation("Got account balance ({id}).", accountBalanceId);

            return Ok(accountBalance);
        }

        [HttpPost(Name = "/exchange")]
        [ProducesResponseType(typeof(AccountBalance), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PastTransaction>> ExchangeCurrenciesBetweenUserAccountsAsync(int srcAccountId, int destAccountId, decimal srcAmount)
        {
            _logger.LogInformation("Preparing currency exchange of amount ({amount}) from account balance ({id}) to account balance ({id}).", srcAccountId, destAccountId, srcAmount);

            _logger.LogInformation("Checking account balances ({id}) and ({id}) exist.", srcAccountId, destAccountId);

            var srcAccountBalance = await _service.GetAccountBalanceByIdAsync(srcAccountId);
            var destAccountBalance = await _service.GetAccountBalanceByIdAsync(destAccountId);

            if (srcAccountBalance == null)
            {
                _logger.LogWarning("Source account balance ({id}) was not found.", srcAccountId);

                return BadRequest($"Source account with Id ({srcAccountId}) was not found. Cannot continue.");
            }

            if (destAccountBalance == null)
            {
                _logger.LogWarning("Destination account balance ({id}) was not found.", destAccountId);

                return BadRequest($"Destination account with Id ({destAccountId}) was not found. Cannot continue.");
            }

            try
            {
                _logger.LogInformation("Both accounts found. Getting exchange rate from currency ({id}) to currency ({id}).", srcAccountBalance.CurrencyId, destAccountBalance.CurrencyId);
                
                GetCurrencyExchangeRateCommand getCurrencyExchangeRateCommand = new GetCurrencyExchangeRateCommand(srcAccountBalance.CurrencyId, destAccountBalance.CurrencyId);
                var exchangeRate = await _mediator.Send(getCurrencyExchangeRateCommand);

                _logger.LogInformation("Got exchange rate. Sending information to perform exchange.");

                CreateCurrencyExchangeCommand createCurrencyExchangeCommand = new CreateCurrencyExchangeCommand(fromAccountBalanceId: srcAccountId, toAccountBalanceId: destAccountId, debitAmount: srcAmount, timeOfRate: exchangeRate.TimeAccessed, exchangeRate: exchangeRate.Rate);
                var result = await _mediator.Send(createCurrencyExchangeCommand);

                _logger.LogInformation("Exchange completed successfully.");
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
