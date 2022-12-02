using BuildGroup.CryptoWallet.App.Contracts;
using BuildGroup.CryptoWallet.App.Contracts.Commands.User;

namespace BuildGroup.CryptoWallet.App.Interfaces;

public interface IUsersService
{
    Task<User> Get(string id);

    Task Delete(string id);

    Task<User> Update(string userId, UpdateUserCommand command);

    Task<User> Create(CreateUserCommand command);

    Task<ICollection<User>> Search();
}