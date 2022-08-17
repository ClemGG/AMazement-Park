using Project.Models.Game.Enums;
using Project.Models.Maze;

namespace Project.ViewModels
{
    public static class GameSession
    {
        public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        public static CustomMazeSettingsSO Settings { get; set; }
        
    }
}