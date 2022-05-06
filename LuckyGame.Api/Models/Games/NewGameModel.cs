using System.ComponentModel.DataAnnotations;

namespace LuckyGame.Api.Models.Games
{
    public class NewGameModel
    {

        [Required]
        [Range(0, 10000)]
        public int Points { get; set; }

        [Required]
        [Range(0, 9)]
        public short Number { get; set; }
    }
}
