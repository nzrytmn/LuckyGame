using AutoMapper;
using LuckyGame.Api.Contracts;
using LuckyGame.Api.Entities;
using LuckyGame.Api.Models.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuckyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameService _gameService;
        private IUserService _userService;
        private IMapper _mapper;

        public GamesController(IGameService gameService, IMapper mapper, IUserService userService)
        {
            _gameService = gameService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("new-game")]
        [Authorize(Roles = "Administrator, User")]
        public  IActionResult NewGame(NewGameModel newGame)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            int userId = int.Parse(User.Identity.Name);

            GameResult? gameResult = _gameService.CalculateAccount(userId, newGame.Number, newGame.Points);
            var model = _mapper.Map<GameResultModel>(gameResult);
            return Ok(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAllGames()
        {
            IEnumerable<Game>? games = _gameService.GetAllGames(null)
                .OrderByDescending(p => p.BetTime);

            if (games == null || games.Count() == 0) return NoContent();

            var model = _mapper.Map<IList<GameModel>>(games);
            return Ok(model);
        }

       
    }
}
