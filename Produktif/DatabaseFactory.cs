using Produktif.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private Database _database;

        public DatabaseFactory()
        {
           
        }
        public Database Get()
        {
            if (_database == null)
            {
                _database = new Database();
                //_database.Database.EnsureDeleted();
                _database.Database.EnsureCreated();
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
