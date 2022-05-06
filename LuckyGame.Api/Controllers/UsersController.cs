using AutoMapper;
using LuckyGame.Api.Contracts;
using LuckyGame.Api.Entities;
using LuckyGame.Api.Helpers;
using LuckyGame.Api.Models.Games;
using LuckyGame.Api.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LuckyGame.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IGameService _gameService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IGameService gameService)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _gameService = gameService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            User? user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Audience = _appSettings.Audience,
                Issuer = _appSettings.Issuer,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }

        [HttpGet()]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAllUsers()
        {
            IEnumerable<User> users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetUser(int id)
        {
            User? user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound("User can not found!");
            }

            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpGet("{userId}/games")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetUserGames(int userId)
        {
            User? user = _userService.GetById(userId);

            if (user == null) return NotFound("There is no such a User");

            IEnumerable<Game>? games = _gameService.GetAllGames(userId)
                .OrderByDescending(p => p.BetTime);

            if (games == null || games.Count() == 0) return NoContent();

            var model = _mapper.Map<IList<GameModel>>(games);
            return Ok(model);
        }

    }
}
