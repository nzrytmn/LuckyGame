using LuckyGame.Api.Enums;

namespace LuckyGame.Api.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool? Status { get; set; }
        public int? Wager { get; set; }
        public DateTime BetTime { get; set; }

    }
}
