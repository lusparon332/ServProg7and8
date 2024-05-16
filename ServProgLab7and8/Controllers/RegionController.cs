using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServProgLab7and8.Models;

namespace ServProgLab7and8.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RegionController : ControllerBase
	{
		private readonly ILogger<RegionController> _logger;
		private readonly VideogamesContext _videogamesContext;

		public RegionController(ILogger<RegionController> logger, VideogamesContext videogamesContext)
		{
			_logger = logger;
			_videogamesContext = videogamesContext;
		}

        [Authorize]
        [HttpGet]
		public async Task<ActionResult<IEnumerable<Region>>> Get()
		{
			return await _videogamesContext.Regions.ToListAsync();

		}

        [Authorize]
        [HttpGet("{id}")]
		public async Task<ActionResult<Region>> Get(int id)
		{
			var res = await _videogamesContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
			if (res == null)
			{
				return NotFound();
			}
			else
				return new ObjectResult(res);
		}

        [Authorize]
        [HttpPost]
		public async Task<ActionResult<Region>> Post(Region item)
		{
			if (item == null)
				return BadRequest();

			_videogamesContext.Regions.Add(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpPut]
		public async Task<ActionResult<Region>> Put(Region item)
		{
			if (item == null)
				return BadRequest();
			if (!_videogamesContext.Regions.Any(x => x.Id == item.Id))
				return NotFound();

			_videogamesContext.Update(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}
	}
}
