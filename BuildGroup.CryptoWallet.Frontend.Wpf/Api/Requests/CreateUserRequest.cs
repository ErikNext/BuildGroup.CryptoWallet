namespace BuildGroup.CryptoWallet.Frontend.Wpf.Api.Requests;

public record CreateUserRequest(string Username, decimal Balance, string CurrencyType);