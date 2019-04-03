namespace Repository.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T> where T : class
    {
        Int32 Count { get; }

        IList<T> GetAll();

        T GetById(object id);

        IList<T> Get(Expression<Func<T, bool>> filter = null,
                          Func<IQueryable<T>, IOrderedQueryable<T>>
                          orderBy = null, string includeProperties = "");

        IList<T> Filter(Expression<Func<T, bool>> predicate);

        IList<T> Filter(Expression<Func<T, bool>> filter,
                             out int total, int index = 0, int size = 50);

        bool Contains(Expression<Func<T, bool>> predicate);

        T Find(params object[] keys);

        T Find(Expression<Func<T, bool>> predicate);

        T Create(T entity);

        void Delete(int id);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        void Update(T entity);

    }
}
