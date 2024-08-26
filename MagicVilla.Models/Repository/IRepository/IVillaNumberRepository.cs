using MagicVilla.Models.Models;

namespace MagicVilla.Models.Repository.IRepository
{
	public interface IVillaNumberRepository : IRepository<VillaNumber>
	{
		Task<VillaNumber> UpdateAsync(VillaNumber entity);
	}
}
