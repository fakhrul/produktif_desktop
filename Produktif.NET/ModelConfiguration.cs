using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produktif.NET
{
    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureActiveAppEntity(modelBuilder);
            ConfigureActiveTimerEntity(modelBuilder);
            ConfigureUserActivityEntity(modelBuilder);
        }

        private static void ConfigureActiveAppEntity(DbModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
