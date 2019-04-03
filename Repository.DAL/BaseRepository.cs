namespace Repository.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Repository.Data;

    public class BaseRepository<T> : IRepository<T>, IDisposable where T : class
    {
        protected readonly MTOContext MTOContext;
        protected readonly DbSet<T> DbSet;
        private bool _isTemp;

        
        public BaseRepository(MTOContext mtoContext = null)
        {
            if (mtoContext == null)
            {
                mtoContext = new MTOContext();
                this._isTemp = true;
            }

            this.DbSet = mtoContext.Set<T>();
            this.MTOContext = mtoContext;
        }

        public virtual int Count
        {
            get { return this.DbSet.Count(); }
        }

        public virtual IList<T> GetAll()
        {
            return this.DbSet.ToList();
        }

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual IList<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = this.DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual IList<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Where(predicate).ToList();
        }

        public virtual IList<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? this.DbSet.Where(filter).AsQueryable() : this.DbSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.ToList();
        }

        public virtual bool Contains(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Count(predicate) > 0;
        }

        public virtual T Find(params object[] keys)
        {
            return this.DbSet.Find(keys);
        }

        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.FirstOrDefault(predicate);

        }

        public virtual T Create(T entity)
        {
            var newEntry = this.DbSet.Add(entity);
            return newEntry;

        }

        public virtual void Delete(int id)
        {
            var entityToDelete = this.DbSet.Find(id);
            this.Delete(entityToDelete);
        }

        public virtual void Delete(T entity)
        {
            if (this.MTOContext.Entry(entity).State == EntityState.Detached)
            {               
                this.DbSet.Attach(entity);
            }

            this.DbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            var entitiesToDelete = this.Filter(predicate);
            foreach (var entity in entitiesToDelete)
            {
                if (this.MTOContext.Entry(entity).State == EntityState.Detached)
                {
                    this.DbSet.Attach(entity);
                }
                this.DbSet.Remove(entity);
            }

        }

        public virtual void Update(T entity)
        {
            var entry = this.MTOContext.Entry(entity);
            this.DbSet.Attach(entity);
            entry.State = EntityState.Modified;

        }

        public void Dispose()
        {
            if (this._isTemp)
                if (this.MTOContext != null)
                    this.MTOContext.Dispose();

        }
    }
}
