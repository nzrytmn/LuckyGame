using LuckyGame.Api.Entities;

namespace LuckyGame.Api.Contracts
{
    public interface IGameService
    {
        GameResult CalculateAccount(int userId, short number, int points);
        IEnumerable<Game> GetAllGames(int? userId);
    }
}
