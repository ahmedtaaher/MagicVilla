using AutoMapper;
using MagicVilla_WebAPI.DTO;
using MagicVilla_WebAPI.Models;
using MagicVilla_WebAPI.Repository;
using MagicVilla_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class VillaController : ControllerBase
	{
		private readonly IVillaRepository villrepository;
		private readonly IMapper mapper;
		protected APIResponse response;
		public VillaController(IVillaRepository villrepository, IMapper mapper)
		{
			this.villrepository = villrepository;
			this.mapper = mapper;
			response = new();
		}
		[HttpGet(Name = "GetVillas")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<APIResponse>> GetVillas()
		{
			try
			{
				IEnumerable<Villa> VillasList = await villrepository.GetAllAsync();
				response.Result = mapper.Map<List<VillaDTO>>(VillasList);
				response.StatusCode = HttpStatusCode.OK;
				return Ok(response);
			}
			catch (Exception e)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { e.ToString() };
				return response;
			}
		}
		[HttpGet("{id:int}", Name = "GetVilla")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<APIResponse>> GetVilla(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				Villa villa = await villrepository.GetAsync(x => x.Id == id);
				if (villa == null)
				{
					return NotFound();
				}
				response.Result = mapper.Map<VillaDTO>(villa);
				response.StatusCode = HttpStatusCode.OK;
				return Ok(response);
			}
			catch (Exception e)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { e.ToString() };
				return response;
			}
		}
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villacreatedto)
		{
			try
			{
				if (await villrepository.GetAsync(c => c.Name.ToLower() == villacreatedto.Name) != null)
				{
					ModelState.AddModelError("Custom error", "Villa already exists");
					return BadRequest();
				}
				Villa villa = mapper.Map<Villa>(villacreatedto);
				await villrepository.CreateAsync(villa);
				response.Result = mapper.Map<VillaDTO>(villa);
				response.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetVilla", new {id = villa.Id}, response);
			}
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { e.ToString() };
				return response;
			}
		}
		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
		{
			try
			{
				if(id == 0)
				{
					return BadRequest();
				}
				Villa villa = await villrepository.GetAsync(z => z.Id == id);
				if(villa == null)
				{
					return NotFound();
				}
				await villrepository.DeleteAsync(villa);
				response.StatusCode = HttpStatusCode.NoContent;
				response.IsSuccess = true;
				return Ok(response);
			}
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { e.ToString() };
				return response;
			}
		}
		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaupdatedto)
		{
			try
			{
				if(villaupdatedto == null || id != villaupdatedto.Id)
				{
					return BadRequest();
				}
				Villa villa = mapper.Map<Villa>(villaupdatedto);
				await villrepository.UpdateAsync(villa);
				response.StatusCode = HttpStatusCode.NoContent;
				response.IsSuccess = true;
				return Ok(response);
			}
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { e.ToString() };
				return response;
			}
		}
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> villaupdatedto)
		{
			if (villaupdatedto == null || id == 0)
			{
				return BadRequest();
			}
			Villa villa = await villrepository.GetAsync(s => s.Id == id, tracked: false);
			VillaUpdateDTO villaUpdateDTO = mapper.Map<VillaUpdateDTO>(villa);
			if (villa == null)
			{
				return BadRequest();
			}
			villaupdatedto.ApplyTo(villaUpdateDTO, ModelState);
			Villa villamodel = mapper.Map<Villa>(villaUpdateDTO);
			await villrepository.UpdateAsync(villamodel);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			return NoContent();
		}

	}
}
