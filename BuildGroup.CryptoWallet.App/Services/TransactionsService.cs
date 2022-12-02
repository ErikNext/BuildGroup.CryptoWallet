using BuildGroup.CryptoWallet.App.Contracts;
using BuildGroup.CryptoWallet.App.Contracts.Commands.Transaction;
using BuildGroup.CryptoWallet.App.Contracts.Items;
using BuildGroup.CryptoWallet.App.Data;
using BuildGroup.CryptoWallet.App.Data.Models;
using BuildGroup.CryptoWallet.App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuildGroup.CryptoWallet.App.Services;

public class TransactionsService : ITransactionsService
{
    private readonly CryptoWalletDbContext _dbContext;
    private readonly IUsersService _usersService;

    public TransactionsService(CryptoWalletDbContext dbContext, IUsersService usersService)
    {
        _dbContext = dbContext;
        _usersService = usersService;
    }

    public async Task<Transaction> Get(string id)
    {
        var row = await GetRow(id);

        return MapToDto(row);
    }

    public async Task Delete(string id)
    {
        var row = await GetRow(id);

        _dbContext.Transactions.Remove(row);

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<Transaction> Create(CreateTransactionCommand command)
    {
        var fromUser = await _usersService.Get(command.FromUserId);
        var toUser = await _usersService.Get(command.ToUserId);

        if (fromUser == null || toUser == null)
            throw new InvalidOperationException(
                $"Can't find fromUser {command.FromUserId} or toUser {command.ToUserId}");

        if (fromUser.CurrencyType != toUser.CurrencyType)
            throw new InvalidOperationException("Can't complete the operation, different currencies");

        if (fromUser.CurrencyType == CurrencyType.Unknown)
            throw new InvalidOperationException("Can't complete the operation, currency type unknown");

        var fromUserNewBalance = fromUser.Balance - command.Amount;
        var toUserNewBalance = toUser.Balance + command.Amount;

        if (fromUserNewBalance < 0)
            throw new InvalidOperationException("Can't complete the operation, there are not enough funds");

        var row = new TransactionRow(
            Ulid.NewUlid().ToString(), 
            command.FromUserId, 
            command.ToUserId,
            command.Amount,
            fromUser.CurrencyType,
            command.TransactionType,
            DateTimeOffset.UtcNow);

        await _usersService.Update(fromUser.Id, new(Balance: fromUserNewBalance));
        await _usersService.Update(toUser.Id, new(Balance: toUserNewBalance));

        await _dbContext.Transactions.AddAsync(row).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return MapToDto(row);
    }

    public async Task<ICollection<Transaction>> Search()
    {
        var collection = await _dbContext.Transactions.ToListAsync();

        return collection.Select(MapToDto).ToList();
    }

    private async Task<TransactionRow> GetRow(string id)
    {
        var row = await _dbContext.Transactions
            .SingleOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

        if (row == null)
        {
            throw new InvalidOperationException($"The Transaction {id} is not found");
        }

        return row;
    }

    private static Transaction MapToDto(TransactionRow row)
    {
        return new Transaction(
            row.Id,
            row.FromUserId,
            row.ToUserId, 
            row.Amount, 
            row.CurrencyType,
            row.TransactionType,
            row.Date);
    }
}