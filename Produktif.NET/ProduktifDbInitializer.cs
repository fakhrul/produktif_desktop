using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produktif
{

    public class ProduktifDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<ProduktifContext>
    {
        public ProduktifDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        { }

        protected override void Seed(ProduktifContext context)
        {
            // Here you can seed your core data if you have any.
        }
    }
}
