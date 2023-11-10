using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PastTransactionController : Controller
    {
        private readonly IPastTransactionService _service;
        private readonly ILogger<PastTransactionController> _logger;

        public PastTransactionController(IPastTransactionService service, ILogger<PastTransactionController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(Name = "/{userTransactionsId}")]
        public async Task<ActionResult<IEnumerable<PastTransaction>>> GetPastTransactionsByUserIdAsync([FromQuery] int userTransactionsId)
        {

            _logger.LogInformation("Getting past transactions for user id ({id})", userTransactionsId);

            var pastTransactions = await _service.GetPastTransactionsByUserIdAsync(userTransactionsId);

            if (pastTransactions == null)
            {
                _logger.LogWarning("Could not find any transactions for user id ({id}).", userTransactionsId);

                return NotFound();
            }

            _logger.LogInformation("Found {count} transactions for user id ({id}).", pastTransactions.Count(), userTransactionsId);

            return Ok(pastTransactions);
        }
    }
}
