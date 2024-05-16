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
	public class GameController : ControllerBase
	{
		private readonly ILogger<GameController> _logger;
		private readonly VideogamesContext _videogamesContext;

		public GameController(ILogger<GameController> logger, VideogamesContext videogamesContext)
		{
			_logger = logger;
			_videogamesContext = videogamesContext;
		}

		[Authorize]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Game>>> Get()
		{
			return await _videogamesContext.Games.ToListAsync();
		}

        [Authorize]
        [HttpGet("{id}")]
		public async Task<ActionResult<Game>> Get(int id)
		{
			var res = await _videogamesContext.Games.FirstOrDefaultAsync(x => x.Id == id);
			if (res == null)
			{
				return NotFound();
			}
			else
			{
				var genre = _videogamesContext.Genres.FirstOrDefault(x => x.Id == res.GenreId);
				genre.Games = null;
				res.Genre = genre;
				return new ObjectResult(res);
			}
		}

        [Authorize]
        [HttpPost]
		public async Task<ActionResult<Game>> Post(Game item)
		{
			if (item == null)
				return BadRequest();

			_videogamesContext.Games.Add(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpPut]
		public async Task<ActionResult<Game>> Put(Game item)
		{
			if (item == null)
				return BadRequest();
			if (!_videogamesContext.Games.Any(x => x.Id == item.Id))
				return NotFound();

			_videogamesContext.Update(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
		public async Task<ActionResult<Game>> Delete(int id)
		{
			var item = _videogamesContext.Games.FirstOrDefault(x => x.Id == id);
			if (item == null)
				return NotFound();
			foreach (var i in _videogamesContext.GamePublishers.Where(x => x.GameId == item.Id))
			{
				i.GameId = null;
			}
			_videogamesContext.Games.Remove(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}
	}
}
