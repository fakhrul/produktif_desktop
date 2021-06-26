using Produktif.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private Database _database;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected Database Database
        {
            get { return _database ?? (_database = _databaseFactory.Get()); }
        }

        public void Commit()
        {
            Database.Commit();
        }
    }
}
