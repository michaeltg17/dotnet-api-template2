using DbUp;
using DbUp.Engine;
using System.Reflection;

namespace Persistence.Migrations;

public static class Migrator
{
    public static DatabaseUpgradeResult Migrate(string connectionString)
    {
        EnsureDatabase.For.SqlDatabase(connectionString, collation: "SQL_Latin1_General_CP1_CI_AS");

        var upgrader = DeployChanges
            .To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .Build();

        return upgrader.PerformUpgrade();
    }
}
