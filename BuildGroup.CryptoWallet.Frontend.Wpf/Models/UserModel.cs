namespace BuildGroup.CryptoWallet.Frontend.Wpf.Models;

public record UserModel(
    string Id,
    string Username, 
    decimal Balance, 
    string CurrencyType);
