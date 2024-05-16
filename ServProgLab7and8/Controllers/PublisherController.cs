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
	public class PublisherController : ControllerBase
	{
		private readonly ILogger<PublisherController> _logger;
		private readonly VideogamesContext _videogamesContext;

		public PublisherController(ILogger<PublisherController> logger, VideogamesContext videogamesContext)
		{
			_logger = logger;
			_videogamesContext = videogamesContext;
		}

        [Authorize]
        [HttpGet]
		public async Task<ActionResult<IEnumerable<Publisher>>> Get()
		{
			return await _videogamesContext.Publishers.ToListAsync();

		}

        [Authorize]
        [HttpGet("{id}")]
		public async Task<ActionResult<Publisher>> Get(int id)
		{
			var res = await _videogamesContext.Publishers.FirstOrDefaultAsync(x => x.Id == id);
			if (res == null)
			{
				return NotFound();
			}
			else
				return new ObjectResult(res);
		}

        [Authorize]
        [HttpPost]
		public async Task<ActionResult<Publisher>> Post(Publisher item)
		{
			if (item == null)
				return BadRequest();

			_videogamesContext.Publishers.Add(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpPut]
		public async Task<ActionResult<Publisher>> Put(Publisher item)
		{
			if (item == null)
				return BadRequest();
			if (!_videogamesContext.Publishers.Any(x => x.Id == item.Id))
				return NotFound();

			_videogamesContext.Update(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
		public async Task<ActionResult<Publisher>> Delete(int id)
		{
			var item = _videogamesContext.Publishers.FirstOrDefault(x => x.Id == id);
			if (item == null)
				return NotFound();
			foreach (var i in _videogamesContext.GamePublishers.Where(i => i.PublisherId == item.Id))
			{
				if (i == null) continue;
				var cur_game = _videogamesContext.Games.FirstOrDefault(g => g.Id == i.GameId);
				foreach (var cur_game_pubs in _videogamesContext.GamePublishers.Where(x => x.GameId == cur_game.Id))
				{
					cur_game_pubs.GameId = null;
				}
				_videogamesContext.Games.Remove(cur_game);
				i.PublisherId = null;
			}
			_videogamesContext.Publishers.Remove(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}
	}
}
