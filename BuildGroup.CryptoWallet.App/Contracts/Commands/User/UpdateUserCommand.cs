using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Contracts.Commands.User;

public record UpdateUserCommand(
    string? Username = null,
    decimal? Balance = null,
    CurrencyType CurrencyType = CurrencyType.Unknown);