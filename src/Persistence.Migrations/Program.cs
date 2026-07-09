using Persistence.Migrations;

var connectionString = "Data Source=127.0.0.1,50000;User ID=sa;Password=yourStrong(!)Password;Trust Server Certificate=True";
Migrator.Migrate(connectionString);