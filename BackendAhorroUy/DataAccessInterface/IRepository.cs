using System;
using System.Collections.Generic;

namespace DataAccessInterface
{
    public interface IRepository<T>
    {
        void Add(T entity);

        void Remove(T entity);

        void Update(T entity);

        IEnumerable<T> GetAll();

        T Get(Guid id);
    }
}
