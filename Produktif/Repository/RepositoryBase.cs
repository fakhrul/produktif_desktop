using Microsoft.EntityFrameworkCore;
using Produktif.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Produktif
{
    public abstract class RepositoryBase<T> where T : class
    {
        private Database _database;
        private readonly DbSet<T> _dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = Database.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get; private set;
        }

        protected Database Database
        {
            get { return _database ?? (_database = DatabaseFactory.Get()); }
        }
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            _database.Entry(entity).State = EntityState.Modified;
        }
        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> All()
        {
            return _dbset.ToList();
        }
        public virtual IEnumerable<T> AllReadOnly()
        {
            return _dbset.AsNoTracking().ToList();
        }
    }
}
