using System;

namespace BuildGroup.CryptoWallet.Frontend.Wpf.Models;

public record TransactionModel(
    string Id,
    string FromUserId,
    string ToUserId,
    decimal Amount,
    string TransactionType,
    string CurrencyType,
    DateTimeOffset Date);