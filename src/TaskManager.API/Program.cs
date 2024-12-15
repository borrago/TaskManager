using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TaskManager.Core.CrossCutting;
using TaskManager.Infra;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

RegisterApi(builder);

var app = builder.Build();

UseApi(app);

await app.RunAsync(new CancellationToken());

#region Facilitadores
static void AddConnections(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<Context>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddSingleton<IMongoClient>(sp =>
        new MongoClient(builder.Configuration["ReadDbSettings:DbConn"]));
    builder.Services.AddSingleton<ContextRead>();

    builder.Services.AddSingleton(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        var databaseName = builder.Configuration["ReadDbSettings:DbName"];
        return client.GetDatabase(databaseName);
    });
}

static void MigrateDataBase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<Program>();
        try
        {
            dbContext.Database.Migrate();
            logger.LogInformation("Banco de dados migrado com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao migrar o banco de dados.");
        }
    }
}

static void RegisterApi(WebApplicationBuilder builder)
{
    builder.Services.Register();

    AddConnections(builder);
}

static void UseApi(WebApplication app)
{
    app.Use();

    app.MapControllers();

    MigrateDataBase(app);
}
#endregion