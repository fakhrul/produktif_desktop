using Produktif.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private ProduktifContext _database;

        public DatabaseFactory()
        {
           
        }
        public ProduktifContext Get()
        {
            if (_database == null)
            {
                _database = new ProduktifContext();
                //_database.Database.EnsureCreated();
            }

            return _database;
        }

        protected override void DisposeCore()
        {
            if (_database != null)
                _database.Dispose();
        }
    }
}
