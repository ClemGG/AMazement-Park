using Project.Core;
using Project.Models.Scenes;
using Project.Procedural.MazeGeneration;
using Project.ViewModels.Entities.Items;
using Project.ViewModels.Scenes;
using UnityEngine;
using MazeGenerator = Project.ViewModels.Generation.MazeGenerator;

namespace Project.ViewModels
{
    /// <summary>
    /// Assigne les correctes valeurs aux monstres, items et joueurs,
    /// et màj la GamSession en cours
    /// </summary>
    public class GameSessionManager : SingletonBehaviour<GameSessionManager>
    {
        #region Public Fields

        [field: SerializeField]
        public SceneToLoadParams VictoryScene { get; set; }

        [field: SerializeField]
        public SceneToLoadParams DefeatScene { get; set; }

        #endregion

        #region Private Fields

        //S'abonner à eux pour appeler leurs events au bon moment
        private MazeGenerator _mazeGenerator;
        private Portal _portalItem;

        #endregion

        #region Mono

        private void Awake()
        {
            _mazeGenerator = FindObjectOfType<MazeGenerator>();
            _mazeGenerator.OnMazeDone += OnMazeDone;
        }

        private void OnDestroy()
        {
            _mazeGenerator.OnMazeDone -= OnMazeDone;
            _portalItem.OnPortalReachedEvent -= OnPortalReached;
        }

        private void Start()
        {
            GameSession.InitNewRun();
        }

        private void Update()
        {
            ComputeElapsedTime();
        }

        #endregion

        #region Public Methods

        //Appelé quand le joueur perd la partie
        public void OnDefeat()
        {
            GameSession.OnDefeat();
            SceneMaster.Instance.LoadSingleSceneAsync(DefeatScene);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calcule le temps passé dans un dédale
        /// </summary>
        private void ComputeElapsedTime()
        {
            if (GameSession.IsActiveTimeReached)
            {
                return;
            }
            else if (GameSession.ElapsedTime >= GameSession.Settings.ActiveTimeLimit)
            {
                GameSession.IsActiveTimeReached = true;
                return;
            }
            GameSession.ElapsedTime += Time.deltaTime;
        }


        #region Events

        /// <summary>
        /// Quand le dédale est créé, on peut récupérer le Portal et s'y abonner
        /// </summary>
        /// <param name="sender">Le MazeGenerator</param>
        /// <param name="e">Pour savoir si le dédale est généré</param>
        private void OnMazeDone(object sender, GenerationProgressReport e)
        {
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                _portalItem = FindObjectOfType<Portal>();
                _portalItem.OnPortalReachedEvent += OnPortalReached;
            }
        }

        /// <summary>
        /// Si le Portal est déverouillé quand atteint, on lance la scène de victoire
        /// et on incrémente le nombre de victoires
        /// </summary>
        /// <param name="sender">Le Portal</param>
        /// <param name="isPortalUnlocked">TRUE si le Portal est actif</param>
        private void OnPortalReached(object sender, bool isPortalUnlocked)
        {
            if (isPortalUnlocked)
            {
                GameSession.OnVictory();
                SceneMaster.Instance.LoadSingleSceneAsync(VictoryScene);
            }
        }

        #endregion

        #endregion
    }
}