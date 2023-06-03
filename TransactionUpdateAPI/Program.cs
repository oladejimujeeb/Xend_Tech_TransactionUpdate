using Microsoft.EntityFrameworkCore;
using UpdateTransaction.Core.BusMessage;
using UpdateTransaction.Core.Context;
using UpdateTransaction.Core.Interfaces.RepositoryInterface;
using UpdateTransaction.Core.Interfaces.ServiceInterface;
using UpdateTransaction.Core.Repositories;
using UpdateTransaction.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string dbConnectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(dbConnectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IWalletRepository,WalletRepository>();
builder.Services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
builder.Services.AddScoped<IWalletTransactionService, WalletTransactionService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITransactionBusMessage, TransactionBusMessage>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
