using ContosoPizza.Data;
using ContosoPizza.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the PizzaContext
// Rsgiters the PizzaContext with ASP.NET Core dependency Injection system
// Specifics that PizzaContext will use the SQLite db provider
// Definesa SQLite connection string that points to a local file called ContosoPizza.db
// **for something local, ok to hard code the connection string, but for Netowrk DBs like
// SQL Server and PostgreSQL, store the connection string securely
// For local dbs, can use Secret Manager
builder.Services.AddSqlite<PizzaContext>("Data Source=ContosoPizza.db");

// Add the PromotionsContext
// This code registers PromotionsContext with the dependency injection system.
builder.Services.AddSqlite<PromotionsContext>("Data Source=Promotions/Promotions.db");

builder.Services.AddScoped<PizzaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Add the CreateDbIfNotExists method call
app.CreateDbIfNotExists();

app.MapGet("/", () => @"Contoso Pizza management API. Navigate to /swagger to open the Swagger test UI.");

app.Run();
