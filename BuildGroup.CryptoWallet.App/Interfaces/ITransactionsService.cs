using BuildGroup.CryptoWallet.App.Contracts;
using BuildGroup.CryptoWallet.App.Contracts.Commands.Transaction;
using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Interfaces;

public interface ITransactionsService
{
    Task<Transaction> Get(string id);

    Task Delete(string id);

    Task<Transaction> Create(CreateTransactionCommand command);

    Task<ICollection<Transaction>> Search();
}