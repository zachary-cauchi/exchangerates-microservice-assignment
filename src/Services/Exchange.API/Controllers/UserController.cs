using Exchange.API.Models;
using Exchange.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly ILogger _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(Name = "{userId}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByIdAsync([FromQuery] int userId)
        {
            var user = await _service.GetUserByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("Tried to get user with id {id} but was not found.", userId);

                return NotFound();
            }

            return Ok(user);
        }
    }
}
