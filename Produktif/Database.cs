using Microsoft.EntityFrameworkCore;
using Produktif.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif
{
    public class Database : DbContext
    {
        public DbSet<ActiveApp> ActiveApp { get; set; }
        public DbSet<ActivityTimer> ActivityTimer { get; set; }
        public DbSet<UserActivity> UserActivity { get; set; }


        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
