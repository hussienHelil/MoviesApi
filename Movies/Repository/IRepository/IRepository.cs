using System.Linq.Expressions;

namespace Movies.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? IncludedNavigations = null, Expression<Func<T,bool>>? predicate = null, Expression<Func<T, string>>? order = null);
        T GetSingleOrDefault(Func<T, bool> predicate);
        T Create(T entity);
        T Update(T entity);

        T Delete(T entity);
    }
}
