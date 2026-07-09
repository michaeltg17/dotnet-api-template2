using Persistence.Migrations;
using Persistence.Migrations.Extensions;

string conn = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");
Migrator.Migrate(conn);