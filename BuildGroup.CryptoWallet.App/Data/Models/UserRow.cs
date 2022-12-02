using BuildGroup.CryptoWallet.App.Contracts.Items;

namespace BuildGroup.CryptoWallet.App.Data.Models;

public class UserRow
{
    public string Id { get; }
    public string Username { get; set; }
    public decimal Balance { get; set; }
    public CurrencyType CurrencyType { get; set; }

    public UserRow(
        string id,
        string username,
        decimal balance,
        CurrencyType currencyType)
    {
        Id = id;
        Username = username;
        Balance = balance;
        CurrencyType = currencyType;
    }
}
//ИЗ минусов: хранил бы лучше баланс и тип валюты в UserAccount, но т.к по условию у нас 1-2 таблички, храню тут)