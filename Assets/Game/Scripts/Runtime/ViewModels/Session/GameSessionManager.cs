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
    /// et m�j la GamSession en cours
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

        //S'abonner � eux pour appeler leurs events au bon moment
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

        //Appel� quand le joueur perd la partie
        public void OnDefeat()
        {
            GameSession.OnDefeat();
            SceneMaster.Instance.LoadSingleSceneAsync(DefeatScene);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calcule le temps pass� dans un d�dale
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
        /// Quand le d�dale est cr��, on peut r�cup�rer le Portal et s'y abonner
        /// </summary>
        /// <param name="sender">Le MazeGenerator</param>
        /// <param name="e">Pour savoir si le d�dale est g�n�r�</param>
        private void OnMazeDone(object sender, GenerationProgressReport e)
        {
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                _portalItem = FindObjectOfType<Portal>();
                _portalItem.OnPortalReachedEvent += OnPortalReached;
            }
        }

        /// <summary>
        /// Si le Portal est d�verouill� quand atteint, on lance la sc�ne de victoire
        /// et on incr�mente le nombre de victoires
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