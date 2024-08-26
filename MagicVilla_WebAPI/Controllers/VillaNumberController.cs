using AutoMapper;
using MagicVilla_WebAPI.DTO;
using MagicVilla_WebAPI.Models;
using MagicVilla_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaNumberController : ControllerBase
	{
		private readonly IVillaNumberRepository villaNumber;
		private readonly IVillaRepository villarepo;
		private readonly IMapper mapper;
		protected APIResponse response;
		public VillaNumberController(IVillaNumberRepository villaNumber, IMapper mapper, IVillaRepository villarepo)
		{
			this.villaNumber = villaNumber;
			this.villarepo = villarepo;
			this.mapper = mapper;
			response = new();
		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetVillaNumbers()
		{
			try
			{
				IEnumerable<VillaNumber> VillaNumberList = await villaNumber.GetAllAsync();
				response.Result = mapper.Map<List<VillaNumberDTO>>(VillaNumberList);
				response.StatusCode = HttpStatusCode.OK;
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { ex.ToString() };
				return response;
			}
		}
		[HttpGet("{id:int}", Name = "GetVillaNumber")]
		public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				VillaNumber villa = await villaNumber.GetAsync(c => c.VillaNo == id);
				if (villa == null)
				{
					return NotFound();
				}
				response.Result = mapper.Map<VillaNumberDTO>(villa);
				response.StatusCode = HttpStatusCode.OK;
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { ex.ToString() };
				return response;
			}
		}
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villanumberdto)
		{
			try
			{
				if (await villaNumber.GetAsync(c => c.VillaNo == villanumberdto.VillaNo) != null)
				{
					ModelState.AddModelError("Custom error", "Villa Number already exists!");
					return BadRequest(ModelState);
				}
				if (await villarepo.GetAsync(z => z.Id == villanumberdto.VillaId) == null)
				{
					ModelState.AddModelError("Custom error", "Villa Id is invalid!");
					return BadRequest(ModelState);
				}
				VillaNumber villa = mapper.Map<VillaNumber>(villanumberdto);
				await villaNumber.CreateAsync(villa);
				response.Result = mapper.Map<VillaNumberDTO>(villa);
				response.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetVilla", new { id = villa.VillaNo }, response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { ex.ToString() };
				return response;
			}
		}
		[HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				VillaNumber villa = await villaNumber.GetAsync(z => z.VillaNo == id);
				if (villa == null)
				{
					return NotFound();
				}
				await villaNumber.DeleteAsync(villa);
				response.StatusCode = HttpStatusCode.NoContent;
				response.IsSuccess = true;
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { ex.ToString() };
				return response;
			}
		}
		[HttpPut("{id:int}", Name = "UpdateVillaNumber")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO villaUpdate)
		{
			try
			{
				if (villaUpdate == null || id != villaUpdate.VillaNo)
				{
					return BadRequest();
				}
				if (await villarepo.GetAsync(z => z.Id == villaUpdate.VillaId) == null)
				{
					ModelState.AddModelError("Custom error", "Villa Id is invalid!");
					return BadRequest(ModelState);
				}
				VillaNumber villa = mapper.Map<VillaNumber>(villaUpdate);
				await villaNumber.UpdateAsync(villa);
				response.StatusCode = HttpStatusCode.NoContent;
				response.IsSuccess = true;
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.ErrorMessages = new List<string>() { ex.ToString() };
				return response;
			}
		}
	}
}
