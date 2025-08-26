using System.Text;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using DbUp.ScriptProviders;
using Microsoft.Extensions.Configuration;

var environment = GetEnvironment();
var config = BuildConfiguration(environment);
var connectionString = GetConnectionString(config);

Console.WriteLine($"Environment: {environment}");

// Common journal table for all schemas
const string JournalTable   = "__schema_history";

// Public schema
const string MigrationsPublicDir  = "Migrations/Public";
const string JournalPublicSchema  = "public";

// Account schema
const string MigrationsAccountDir  = "Migrations/Account";
const string JournalAccountSchema  = "account";

const string RepeatablesDir = "Repeatable";

ValidateDirectories(MigrationsPublicDir);
ValidateConnectionString(connectionString);

EnsureDatabase.For.PostgresqlDatabase(connectionString);

RunMigrations(connectionString, MigrationsPublicDir, JournalPublicSchema, JournalTable);
// RunMigrations(connectionString, MigrationsAccountDir, JournalAccountSchema, JournalTable); // No migrations yet

RunRepeatables(connectionString, RepeatablesDir);

Environment.Exit(0);
return;


static string GetEnvironment()
    => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

static IConfigurationRoot BuildConfiguration(string environment)
    => new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

static string GetConnectionString(IConfigurationRoot config)
    => Environment.GetEnvironmentVariable("ConnectionStringApplication")
       ?? config.GetConnectionString("Application")
       ?? throw new InvalidOperationException("Brak ConnectionStringApplication / Application.");


static void ValidateDirectories(params string[] dirs)
{
    foreach (var d in dirs)
    {
        if (!Directory.Exists(d))
        {
            Console.Error.WriteLine($"Directory does not exist: {d}");
            Environment.Exit(2);
        }
    }
}

static void ValidateConnectionString(string cs)
{
    if (string.IsNullOrWhiteSpace(cs))
    {
        Console.Error.WriteLine("Empty connection string.");
        Environment.Exit(2);
    }
}

static void RunMigrations(
    string connectionString,
    string scriptsDir,
    string journalSchema,
    string journalTable)
{
    var upgrader = DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .JournalToPostgresqlTable(journalSchema, journalTable)
        .WithScriptsFromFileSystem(scriptsDir, new FileSystemScriptOptions
        {
            Encoding = Encoding.UTF8
        })
        .LogToConsole()
        .Build();

    if (!upgrader.IsUpgradeRequired())
    {
        Console.WriteLine("No migration scripts to run.");
        return;
    }

    var result = upgrader.PerformUpgrade();
    if (!result.Successful)
    {
        PrintError(result);
        Environment.Exit(1);
    }

    PrintSuccess("Migration scripts success!");
}

static void RunRepeatables(string connectionString, string scriptsDir)
{
    if (!Directory.Exists(scriptsDir))
    {
        Console.WriteLine("No repeatable directory found – skipping.");
        return;
    }

    var upgrader = DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .JournalTo(new NullJournal())
        .WithScriptsFromFileSystem(scriptsDir, new FileSystemScriptOptions
        {
            IncludeSubDirectories = true,
            Encoding = Encoding.UTF8
        })
        .LogToConsole()
        .WithTransactionPerScript()
        .Build();

    var result = upgrader.PerformUpgrade();
    if (!result.Successful)
    {
        PrintError(result);
        Environment.Exit(1);
    }

    PrintSuccess("Repeatable scripts success!");
}

static void PrintError(DatabaseUpgradeResult result)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
}

static void PrintSuccess(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(message);
    Console.ResetColor();
}