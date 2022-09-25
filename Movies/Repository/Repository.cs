using Microsoft.EntityFrameworkCore;
using Movies.Model;
using Movies.Repository.IRepository;
using System.Linq.Expressions;

namespace Movies.Repository
{
    public class Repository<T> : IRepository<T> where T : class 
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;


        public Repository(ApplicationDbContext db)
        {
            this._db = db;
            _dbSet = db.Set<T>();
        }
        public T Create(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }


        public IEnumerable<T> GetAll(string? IncludedNavigations = null, Func<T, bool>? predicate = null, Expression<Func<T, string>>? order = null)
        {
            IQueryable<T> result = _dbSet;
            if (predicate != null)
            {
                result = result.Where(predicate).AsQueryable<T>();
            }
            if (IncludedNavigations != null && IncludedNavigations.Trim() != "")
            {
                string[] models = IncludedNavigations.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in models)
                {
                    result = result.Include(item);
                }
            }
            if (order != null)
            {
                result = result.OrderByDescending(order);
            }
            return result;
        }

        public T GetSingleOrDefault(Func<T, bool> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
    }
}
