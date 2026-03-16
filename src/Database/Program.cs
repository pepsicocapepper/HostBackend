using System.Drawing;
using System.Reflection;
using DbUp;

namespace Database;

internal abstract class Program
{
    private static int Main()
    {
        var env = Environment.GetEnvironmentVariable("ENVIRONMENT");
        var connectionString = Environment.GetEnvironmentVariable("HOST_DB_ADMIN_CONNECTION");
        if (connectionString == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Connection string expected: Database.exe <connection-string>");
            Console.ResetColor();
            return -1;
        }

        DropDatabase.For.PostgresqlDatabase(connectionString);
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        DbUp.Engine.UpgradeEngine upgrader = DeployChanges
            .To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.Contains(".Scripts."))
            .WithTransaction()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            return -1;
        }

        if (env is "Development" or "Staging")
        {
            var seeder = DeployChanges.To.PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.Contains(".Seeds."))
                .WithTransaction()
                .LogToConsole()
                .JournalToPostgresqlTable("public", "seeds_journal")
                .Build();
            var seederResult = seeder.PerformUpgrade();
            if (!seederResult.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(seederResult.Error);
            }

            if (env is "Staging")
            {
                var permissions = DeployChanges.To.PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                        script => script.Contains(".Permissions."))
                    .WithTransaction()
                    .LogToConsole()
                    .Build();
                var permissionsResult = permissions.PerformUpgrade();
                if (!permissionsResult.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(permissionsResult.Error);
                }
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Migration succeeded for {0}", connectionString);

        Console.ResetColor();
        return 0;
    }
}