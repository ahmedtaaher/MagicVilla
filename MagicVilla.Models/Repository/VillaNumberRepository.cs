using MagicVilla_WebAPI.Models;
using MagicVilla_WebAPI.Repository.IRepository;
using MagicVilla_WebAPI.Services;

namespace MagicVilla.Models.Repository
{
	public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
	{
		private readonly AppDbContext Context;
		public VillaNumberRepository(AppDbContext Context) : base(Context)
		{
			this.Context = Context;
		}

		public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
		{
			entity.UpdatedDate = DateTime.Now;
			Context.Update(entity);
			await SaveAsync();
			return entity;
		}
	}
}
