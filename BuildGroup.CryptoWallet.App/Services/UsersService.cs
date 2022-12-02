using BuildGroup.CryptoWallet.App.Contracts;
using BuildGroup.CryptoWallet.App.Contracts.Commands.User;
using BuildGroup.CryptoWallet.App.Contracts.Items;
using BuildGroup.CryptoWallet.App.Data;
using BuildGroup.CryptoWallet.App.Data.Models;
using BuildGroup.CryptoWallet.App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuildGroup.CryptoWallet.App.Services;

public class UsersService : IUsersService
{
    private readonly CryptoWalletDbContext _dbContext;

    public UsersService(CryptoWalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Get(string id)
    {
        var row = await GetRow(id);

        return MapToDto(row);
    }

    public async Task Delete(string id)
    {
        var row = await GetRow(id);

        _dbContext.Users.Remove(row);

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<User> Update(
        string userId,
        UpdateUserCommand command)
    {
        var row = await GetRow(userId);

        row.Username = command.Username ?? row.Username;
        row.Balance = command.Balance ?? row.Balance;
        row.CurrencyType = command.CurrencyType == CurrencyType.Unknown ? row.CurrencyType : command.CurrencyType;

        _dbContext.Users.Update(row);

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return MapToDto(row);
    }

    public async Task<User> Create(CreateUserCommand command)
    {
        var row = new UserRow(
            Ulid.NewUlid().ToString(), 
            command.Username, 
            command.Balance, 
            command.CurrencyType);

        await _dbContext.Users.AddAsync(row).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return MapToDto(row);
    }

    public async Task<ICollection<User>> Search()
    {
        var collection = await _dbContext.Users.ToListAsync();

        return collection.Select(MapToDto).ToList();
    }

    private static User MapToDto(UserRow row)
    {
        return new User(
            row.Id, 
            row.Username, 
            row.Balance,
            row.CurrencyType);
    }

    private async Task<UserRow> GetRow(string id)
    {
        var row = await _dbContext.Users
            .SingleOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

        if (row == null)
        {
            throw new InvalidOperationException($"The User {id} is not found");
        }

        return row;
    }
}