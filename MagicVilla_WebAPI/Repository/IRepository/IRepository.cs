using System.Linq.Expressions;

namespace MagicVilla_WebAPI.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true);
		Task<IEnumerable<T>> GetAllAsync();
		Task CreateAsync(T entity);
		Task DeleteAsync(T entity);
		Task SaveAsync();
	}
}
