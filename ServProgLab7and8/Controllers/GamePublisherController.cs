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
	public class GamePublisherController : ControllerBase
	{
		private readonly ILogger<GamePublisherController> _logger;
		private readonly VideogamesContext _videogamesContext;

		public GamePublisherController(ILogger<GamePublisherController> logger, VideogamesContext videogamesContext)
		{
			_logger = logger;
			_videogamesContext = videogamesContext;
		}

        [Authorize]
        [HttpGet]
		public async Task<ActionResult<IEnumerable<GamePublisher>>> Get()
		{
			return await _videogamesContext.GamePublishers.ToListAsync();

		}

        [Authorize]
        [HttpGet("{id}")]
		public async Task<ActionResult<GamePublisher>> Get(int id)
		{
			var res = await _videogamesContext.GamePublishers.FirstOrDefaultAsync(x => x.Id == id);
			if (res == null)
			{
				return NotFound();
			}
			else
			{
				res.GamePlatforms = null;
				var game = _videogamesContext.Games.FirstOrDefault(x => x.Id == res.GameId);
				game.GamePublishers = null;
				game.Genre = null;
				res.Game = game;
				var publisher = _videogamesContext.Publishers.FirstOrDefault(x => x.Id == res.PublisherId);
				publisher.GamePublishers = null;
				res.Publisher = publisher;
				return new ObjectResult(res);
			}
		}

        [Authorize]
        [HttpPost]
		public async Task<ActionResult<GamePublisher>> Post(GamePublisher item)
		{
			if (item == null)
				return BadRequest();

			_videogamesContext.GamePublishers.Add(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpPut]
		public async Task<ActionResult<GamePublisher>> Put(GamePublisher item)
		{
			if (item == null)
				return BadRequest();
			if (!_videogamesContext.GamePublishers.Any(x => x.Id == item.Id))
				return NotFound();
			
			item.Publisher = null;
			item.GamePlatforms = null;
			item.Game = null;
			_videogamesContext.Update(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
		public async Task<ActionResult<GamePublisher>> Delete(int id)
		{
			var item = _videogamesContext.GamePublishers.FirstOrDefault(x => x.Id == id);
			if (item == null)
				return NotFound();
			foreach (var i in _videogamesContext.Games.Where(x => x.Id == item.GameId))
			{
				_videogamesContext.Games.Remove(i);
			}
			/*foreach (var i in _videogamesContext.Publishers.Where(x => x.Id == item.PublisherId))
			{
				_videogamesContext.Publishers.Remove(i);
			}*/
			_videogamesContext.GamePublishers.Remove(item);
			await _videogamesContext.SaveChangesAsync();
			return Ok(item);
		}
	}
}
