using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public interface IRepository<T> where T : class, IDatabased
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);

        IEnumerable<T> Init(IEnumerable<T> collection);

    }
}
