using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Contracts;

public record Transaction(
    string Id, 
    string FromUserId,
    string ToUserId, 
    decimal Amount,
    CurrencyType CurrencyType,
    TransactionType TransactionType, 
    DateTimeOffset Date);