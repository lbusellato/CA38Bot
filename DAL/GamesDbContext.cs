using Ca38Bot.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Ca38Bot.DAL
{
    [DbConfigurationType(typeof(GamesDbConfiguration))]
    public class GamesDbContext : DbContext
    {
        public static string connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\loren\\Documents\\Games.mdf;Integrated Security = True; Connect Timeout = 30";

        public GamesDbContext() : base(connString)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PartecipantiDbContext, avenabot.Migrations.PartecipantiMigrations.Configuration>());
        }

        public DbSet<Games> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}