using Project.Models.Game.Enums;

namespace Project.ViewModels
{
    public static class GameSession
    {
        public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        
    }
}