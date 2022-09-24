using Project.Models.Game.Enums;
using Project.Models.Maze;

namespace Project.ViewModels
{
    public static class GameSession
    {
        #region Public Fields

        public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        public static CustomMazeSettingsSO Settings { get; set; }

        #endregion


        #region Accessors

        public static bool IsGamePaused { get; set; } = false;

        #endregion


    }
}