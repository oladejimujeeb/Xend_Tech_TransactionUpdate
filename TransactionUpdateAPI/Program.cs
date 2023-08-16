using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UpdateTransaction.Core.BusMessage;
using UpdateTransaction.Core.Context;
using UpdateTransaction.Core.Interfaces.RepositoryInterface;
using UpdateTransaction.Core.Interfaces.ServiceInterface;
using UpdateTransaction.Core.NotificationHub;
using UpdateTransaction.Core.Repositories;
using UpdateTransaction.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string dbConnectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(dbConnectionString);
});
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services.AddHealthChecks();
/*.AddSqlServer(
                Configuration.GetConnectionString("Default"),
                name: $"{title}-db-check",
                tags: new string[] { title.ToLower() });*/
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});
builder.Services.AddControllers();
builder.Services.AddSignalR();

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
builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,

        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.MapHealthChecksUI(options => options.UIPath = "/dashboard");
app.UseCors("CORSPolicy");
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>("/newuser");
    //endpoints.MapHub<MessageHub>()
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
