using System.Text.Json.Serialization;
using BuildGroup.CryptoWallet.App.Data;
using BuildGroup.CryptoWallet.App.Interfaces;
using BuildGroup.CryptoWallet.App.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.AllowTrailingCommas = true;
    });

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CryptoWalletDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CryptoWalletDb")));

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();


app.Run();


