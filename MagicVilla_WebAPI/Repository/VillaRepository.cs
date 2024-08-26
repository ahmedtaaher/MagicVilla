using MagicVilla_WebAPI.Models;
using MagicVilla_WebAPI.Repository.IRepository;
using MagicVilla_WebAPI.Services;

namespace MagicVilla_WebAPI.Repository
{
	public class VillaRepository : Repository<Villa>, IVillaRepository
	{
		private readonly AppDbContext Context;

		public VillaRepository(AppDbContext Context) : base(Context)
		{
			this.Context = Context;
		}

		public async Task<Villa> UpdateAsync(Villa entity)
		{
			entity.UpdatedDate = DateTime.Now;
			Context.Update(entity);
			await SaveAsync();
			return entity;
		}
	}
}
