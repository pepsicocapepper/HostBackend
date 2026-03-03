using System.Drawing;
using System.Reflection;
using DbUp;

namespace Database;

internal abstract class Program
{
    private static int Main()
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        if (connectionString == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Connection string expected: Database.exe <connection-string>");
            Console.ResetColor();
            return -1;
        }

        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        DbUp.Engine.UpgradeEngine upgrader = DeployChanges
            .To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Migration succeeded for {0}", connectionString);

        Console.ResetColor();
        return 0;
    }
}