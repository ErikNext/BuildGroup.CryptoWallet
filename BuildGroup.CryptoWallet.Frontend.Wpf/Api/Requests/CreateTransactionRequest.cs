namespace BuildGroup.CryptoWallet.Frontend.Wpf.Api.Requests;

public record CreateTransactionRequest(
    string FromUserId, 
    string ToUserId, 
    decimal Amount,
    string TransactionType);