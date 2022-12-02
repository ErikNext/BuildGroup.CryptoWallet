using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Contracts.Commands.User;

public record CreateUserCommand(
    string Username,
    decimal Balance,
    CurrencyType CurrencyType);