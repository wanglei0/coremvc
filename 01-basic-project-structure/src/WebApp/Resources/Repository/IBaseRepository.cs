using System;
using System.Collections.Generic;
using WebApp.Resources.Providers;

namespace WebApp.Resources.Repository
{
    public interface IBaseRepository<T>
    {
        List<T> GetAll();

        T GetById(Guid id);

        T Insert(T entity);

        void Update(T entity);


        void Delete(Guid id);

    }
}