using Project.Models.Game.Enums;
using Project.Models.Maze;

namespace Project.ViewModels
{
    public static class GameSession
    {
        #region Public Fields

        public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        public static CustomMazeSettingsSO Settings { get; set; }
        public static bool IsGamePaused { get; set; } = false;

        /// <summary>
        /// Combien de temps s'est écoulé depuis le début de la Session
        /// </summary>
        public static float ElapsedTime { get; set; } = 0f;

        /// <summary>
        /// True si le temps passé dans le dédale dépasse le ActiveTimeLimit
        /// dans les paramètres
        /// </summary>
        public static bool IsActiveTimeReached { get; set; } = false;

        #endregion

        #region Accessors

        /// <summary>
        /// Combien de niveaux a-t'on traversé ?
        /// </summary>
        public static int NbLevelWon { get; private set; } = 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// Remet la Session à 0 pour une nouvelle traversée d'un dédale
        /// </summary>
        public static void InitNewSession()
        {
            ElapsedTime = 0f;
        }

        #endregion
    }
}