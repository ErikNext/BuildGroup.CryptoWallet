using BuildGroup.CryptoWallet.App.Contracts;
using BuildGroup.CryptoWallet.App.Contracts.Commands.User;
using BuildGroup.CryptoWallet.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BuildGroup.CryptoWallet.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Create(
            [FromBody] CreateUserCommand command)
        {
            var result = await _service
                .Create(command)
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Update(
            [FromRoute] string id,
            [FromBody] UpdateUserCommand command)
        {
            var result = await _service
                .Update(id, command)
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Get(
            [FromRoute] string id)
        {
            var result = await _service
                .Get(id)
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<User>>> Search()
        {
            var result = await _service
                .Search()
                .ConfigureAwait(false);

            return Ok(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            await _service
                .Delete(id)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}