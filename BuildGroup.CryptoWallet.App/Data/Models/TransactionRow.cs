using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Data.Models;

public class TransactionRow
{
    public string Id { get; }
    public string FromUserId { get; }
    public string ToUserId { get; }
    public decimal Amount { get; }
    public CurrencyType CurrencyType { get; }
    public TransactionType TransactionType { get; }
    public DateTimeOffset Date { get; }

    public TransactionRow(
        string id, 
        string fromUserId,
        string toUserId,
        decimal amount, 
        CurrencyType currencyType, 
        TransactionType transactionType,
        DateTimeOffset date)
    {
        Id = id;
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Amount = amount;
        CurrencyType = currencyType;
        TransactionType = transactionType;
        Date = date;
    }
}

//Хранил бы лучше баланс и тип валюты в UserAccount, но т.к по условию у нас 1-2 таблички, храню тут)