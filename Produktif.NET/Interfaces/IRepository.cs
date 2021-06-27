using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Produktif.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(long Id);
        IEnumerable<T> All();
        IEnumerable<T> AllReadOnly();
    }
}
