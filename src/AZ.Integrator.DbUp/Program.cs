using DbUp;
using DbUp.Helpers;
using DbUp.ScriptProviders;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStringApplication")
                       ?? config.GetConnectionString("Application");

Console.WriteLine($"Environment: {environment}");

const string oneTimeScriptsDirectory = "OneTime";
const string permanentScriptsDirectory = "Permanent";

const ConsoleColor errorConsoleColor = ConsoleColor.Red;
const ConsoleColor successConsoleColor = ConsoleColor.Green;

EnsureDatabase.For.PostgresqlDatabase(connectionString);

// var oneTimeScriptsUpgrader =
//     DeployChanges.To
//         .PostgresqlDatabase(connectionString)
//         .WithScriptsFromFileSystem(oneTimeScriptsDirectory)
//         .LogToConsole()
//         .Build();
//
// if (oneTimeScriptsUpgrader.IsUpgradeRequired())
// {
//     var oneTimeResult = oneTimeScriptsUpgrader.PerformUpgrade();
//
//     if (!oneTimeResult.Successful)
//     {
//         Console.ForegroundColor = errorConsoleColor;
//         Console.WriteLine(oneTimeResult.Error);
//         Console.ResetColor();
// #if DEBUG
//         Console.ReadLine();
// #endif                
//     }
//     else
//     {
//         Console.ForegroundColor = successConsoleColor;
//         Console.WriteLine("One Time scripts success!");
//         Console.ResetColor();
//     }
// }

var permanentScriptsUpgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsFromFileSystem(permanentScriptsDirectory, new FileSystemScriptOptions
        {
            IncludeSubDirectories = true
        })
        .JournalTo(new NullJournal())
        .LogToConsole()
        .Build();

if (permanentScriptsUpgrader.IsUpgradeRequired())
{
    var permanentResult = permanentScriptsUpgrader.PerformUpgrade();

    if (!permanentResult.Successful)
    {
        Console.ForegroundColor = errorConsoleColor;
        Console.WriteLine(permanentResult.Error);
        Console.ResetColor();
#if DEBUG
        Console.ReadLine();
#endif                
        return -1;
    }

    Console.ForegroundColor = successConsoleColor;
    Console.WriteLine("Permanent scripts success!");
    Console.ResetColor();
}

return 0;