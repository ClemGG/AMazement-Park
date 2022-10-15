using System;
using System.Collections.Generic;
using System.Linq;
using Project.Models.Game.Enums;
using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;

namespace Project.ViewModels
{
    public static class GameSession
    {
        #region Public Fields

        public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        public static CustomMazeSettingsSO Settings { get; set; }
        public static bool IsGamePaused { get; set; } = false;

        /// <summary>
        /// Combien de temps s'est �coul� depuis le d�but de la Session
        /// </summary>
        public static float ElapsedTime { get; set; } = 0f;

        /// <summary>
        /// True si le temps pass� dans le d�dale d�passe le ActiveTimeLimit
        /// dans les param�tres
        /// </summary>
        public static bool IsActiveTimeReached { get; set; } = false;

        /// <summary>
        /// TRUE si le Tricheur a �t� utilis� pour finir le d�dale
        /// </summary>
        public static bool WasCheaterUsed { get; set; } = false;

        /// <summary>
        /// Emp�che d'ajouter un monstre s'il y en a d�j� eu un la session pr�c�dente.
        /// </summary>
        public static bool WasMonsterSpawnedLastLevel { get; set; } = false;

        #endregion

        #region Accessors

        /// <summary>
        /// Combien de niveaux a-t'on travers� ?
        /// </summary>
        public static int NbLevelsWon { get; private set; } = 0;

        /// <summary>
        /// Combien d'objets le joueur a ramass� dans cette session ?
        /// </summary>
        public static int NbItemsUsed { get; private set; } = 0;

        public static string TimeInMinutesSeconds
        {
            get => TimeSpan.FromSeconds(ElapsedTime).ToString("m\\:ss");
        }

        public static bool HasReachedTimeLimit
        {
            get => ElapsedTime > 900f;
        }

        public static bool WasLastLevel
        {
            get => NbLevelsWon == Settings.MaxNumberOfRuns;
        }

        /* Doit ajouter un autre monstre si : 
         * Le jeu est en mode Normal, Difficile ou Cauchemar,
         * Le temps pass� d�passe 15 minutes,
         * Le Tricheur a �t� utilis�,
         * Aucun monstre n'a �t� ajout� au dernier tour,
         * OU le nombre de niveaux est pair
         */
        public static bool ShouldSpawnNewMonster
        {
            get
            {
                bool correctDifficulty = DifficultyLevel == Difficulty.Normal ||
                                         DifficultyLevel == Difficulty.Hard;
                return correctDifficulty && !WasMonsterSpawnedLastLevel && 
                    (
                       HasReachedTimeLimit ||
                       WasCheaterUsed ||
                       NbLevelsWon % (NbItemsUsed > 1 ? 2 : 4) == 0
                    ); 
            }
        }

        /// <summary>
        /// Les valeurs des monstres, copi�s � la g�n�ration des param�tres.
        /// </summary>
        public static int[] ActivityLevels { get; private set; } = new int[]
        {
            0,
            0,
            0,
            0,
            0
        };

        #endregion

        #region Private Fields

        private static bool _isNewSession = true;

        #endregion

        #region Public Methods

        /// <summary>
        /// Appel�e pour nettoyer le GameSession de la partie pr�c�dent
        /// (par ex: Quand on retourne au menu)
        /// </summary>
        public static void ResetGameSession()
        {
            ActivityLevels = new int[]
            {
                -1,
                -1,
                -1,
                -1,
                -1
            };
            InitNewRun();
            _isNewSession = true;
        }

        /// <summary>
        /// Remet la Session � 0 pour une nouvelle travers�e d'un d�dale
        /// </summary>
        public static void InitNewRun()
        {
            ElapsedTime = 0f;
            IsActiveTimeReached = DifficultyLevel == Difficulty.Nightmare;
            WasCheaterUsed = false;
            NbItemsUsed = 0;

            if (_isNewSession)
            {
                _isNewSession = false;
                ActivityLevels = Settings.ActivityLevels;
            }
        }


        /// <summary>
        /// Quand le joueur a r�ussi un d�dale
        /// </summary>
        public static void OnVictory()
        {
            NbLevelsWon++;
            PlayerPrefs.SetInt($"{DifficultyLevel}_victories", NbLevelsWon);
        }

        /// <summary>
        /// Ajoute un nouveau monstre au hasard
        /// </summary>
        public static void AddNewRandomMonster()
        {
            //S'il reste au moins 1 monstre qui n'a pas �t� activ�
            if(ActivityLevels.Any(i => i == 0))
            {
                List<int> randomIndexes = new();
                for (int i = 0; i < ActivityLevels.Length; i++)
                {
                    if(ActivityLevels[i] == 0)
                    {
                        randomIndexes.Add(i);
                    }
                }

                //On prend un index au hasard.
                //Cette m�thode n'est appel�e qu'en mode Normal ou Difficile,
                //donc on ne craint rien si on code l'assignement en dur.
                int alea = randomIndexes.Sample();
                randomIndexes[alea] = DifficultyLevel == Difficulty.Normal ? 3 : 4;
            }
        }

        #endregion
    }
}