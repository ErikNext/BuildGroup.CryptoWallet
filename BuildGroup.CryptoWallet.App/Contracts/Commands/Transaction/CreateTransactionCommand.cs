using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Contracts.Commands.Transaction;

public record CreateTransactionCommand(
    string FromUserId,
    string ToUserId,
    decimal Amount,
    TransactionType TransactionType);