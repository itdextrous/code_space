using MongoDB.Driver;
using MongoDBMigrations;

namespace SAASCLOUDAPP.DataAccessLayer
{
    public class MigrationsHelper
    {
        public static void MigrateToLatestVersion(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            // MongoDBMigrations.MigrationNotFoundException may be thrown if there are '_migrations' in the
            // database collection that do not match the migrations specified in this assembly.
            new MigrationEngine().UseDatabase(connectionString, url.DatabaseName)
                .UseAssemblyOfType<MigrationsHelper>()
                .UseSchemeValidation(false)
                .Run();
        }
    }
}
