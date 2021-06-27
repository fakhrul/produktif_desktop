using Produktif.Models;
using System.Data.Common;
using System.Data.Entity;

namespace Produktif
{
    public class ProduktifContext : DbContext
    {
        public DbSet<ActiveApp> ActiveApp { get; set; }
        public DbSet<ActivityTimer> ActivityTimer { get; set; }
        public DbSet<UserActivity> UserActivity { get; set; }

        public ProduktifContext()
            :this(@"data source=.\data.sqlite;foreign keys=true")
        {

        }
        public ProduktifContext(string nameOrConnectionString)
            :base(nameOrConnectionString)
        {
            Configure();
        }

        public ProduktifContext(DbConnection connection, bool contextOwnsConnection)
           : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var initializer = new ProduktifDbInitializer(modelBuilder);
            Database.SetInitializer(initializer);
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ProduktifContext>(modelBuilder);
        //    Database.SetInitializer(sqliteConnectionInitializer);
        //}

        //protected override void OnConfiguring(
        //    DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=data.db");
        //    optionsBuilder.UseLazyLoadingProxies();
        //    base.OnConfiguring(optionsBuilder);
        //}

        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
