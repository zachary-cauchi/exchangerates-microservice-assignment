using Exchange.API.Models;
using Exchange.API.Services;
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
            var pastTransactions = await _service.GetPastTransactionsByUserIdAsync(userTransactionsId);

            if (pastTransactions == null)
            {
                return NotFound();
            }

            return Ok(pastTransactions);
        }
    }
}
