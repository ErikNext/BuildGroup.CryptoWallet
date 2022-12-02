using BuildGroup.CryptoWallet.App.Contracts;
using BuildGroup.CryptoWallet.App.Contracts.Commands.Transaction;
using BuildGroup.CryptoWallet.App.Contracts.Items;
using BuildGroup.CryptoWallet.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BuildGroup.CryptoWallet.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _service;

        public TransactionsController(ITransactionsService service)
        {
            _service = service;
        }

        [HttpGet("CurrencyTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<CurrencyType>>> GetCurrencyTypes()
        {
            var types = Enum.GetValues<CurrencyType>()
                .Where(t => t != CurrencyType.Unknown);

            return Ok(types);
        }

        [HttpGet("TransactionTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<CurrencyType>>> GetTransactionTypes()
        {
            var types = Enum.GetValues<TransactionType>()
                .Where(t => t != TransactionType.Unknown);

            return Ok(types);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Transaction>> Create(
            [FromBody] CreateTransactionCommand command)
        {
            var result = await _service
                .Create(command)
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Transaction>> Get(
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
        public async Task<ActionResult<ICollection<Transaction>>> Search()
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