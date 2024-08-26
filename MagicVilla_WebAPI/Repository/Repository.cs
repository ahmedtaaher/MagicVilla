using MagicVilla_WebAPI.Repository.IRepository;
using MagicVilla_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_WebAPI.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext Context;
		internal DbSet<T> dbset;
        public Repository(AppDbContext Context)
        {
            this.Context = Context;
			dbset = Context.Set<T>();
        }
        public async Task CreateAsync(T entity)
		{
			await dbset.AddAsync(entity);
			await SaveAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			dbset.Remove(entity);
			await SaveAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			IQueryable<T> query = dbset;
			return await query.ToListAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true)
		{
			IQueryable<T> query = dbset;
			if(!tracked)
			{
				query = query.AsNoTracking();
			}
			if(filter != null)
			{
				query = query.Where(filter);
			}
			return await query.FirstOrDefaultAsync();
		}

		public async Task SaveAsync()
		{
			await Context.SaveChangesAsync();
		}
	}
}
