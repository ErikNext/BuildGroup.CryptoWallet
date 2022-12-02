namespace BuildGroup.CryptoWallet.Frontend.Wpf.Api.Requests;

public record UpdateUserRequest(string Username, decimal Balance, string CurrencyType);