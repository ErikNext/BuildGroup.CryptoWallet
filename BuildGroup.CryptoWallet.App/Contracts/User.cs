using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Contracts;

public record User(
    string Id,
    string Username, 
    decimal Balance, 
    CurrencyType CurrencyType);