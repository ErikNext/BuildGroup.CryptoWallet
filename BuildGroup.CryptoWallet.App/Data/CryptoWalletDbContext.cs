using BuildGroup.CryptoWallet.App.Contracts.Items;
using BuildGroup.CryptoWallet.App.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildGroup.CryptoWallet.App.Data;

public class CryptoWalletDbContext : DbContext
{
    public CryptoWalletDbContext(DbContextOptions<CryptoWalletDbContext> options) : base(options)
    {
    }

    public DbSet<UserRow> Users { get; set; }
    public DbSet<TransactionRow> Transactions { get; set; }
    public string Id { get; }
    public string FromUserId { get; }
    public string ToUserId { get; }
    public decimal Amount { get; }
    public CurrencyType CurrencyType { get; }
    public TransactionType TransactionType { get; }
    public DateTimeOffset Date { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRow>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username);
            entity.Property(e => e.Balance);
            entity.Property(e => e.CurrencyType);
        });

        modelBuilder.Entity<TransactionRow>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FromUserId);
            entity.Property(e => e.ToUserId);
            entity.Property(e => e.Amount);
            entity.Property(e => e.CurrencyType);
            entity.Property(e => e.TransactionType);
            entity.Property(e => e.Date);
        });
    }
}