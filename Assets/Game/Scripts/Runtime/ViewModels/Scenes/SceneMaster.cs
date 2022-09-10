using Project.Core;
using Project.Models.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.ViewModels.Scenes
{
    /// <summary>
    /// Gère le changement de scènes et le chargement de scènes en additif
    /// </summary>
    public class SceneMaster : SingletonBehaviour<SceneMaster>
    {
        #region Public Fields


        /// <summary>
        /// La première scène active quand le SceneMaster est initialisé.
        /// </summary>
        [field: Space(10)]
        [field: Header("Scene Parameters")]
        [field: SerializeField]
        private SceneToLoadParams InitialFirstActiveScene { get; set; }

        /// <summary>
        /// Les scènes additives à charger quand le SceneMaster est initialisé.
        /// </summary>
        [field: Space(10)]
        [field: SerializeField]
        private SceneToLoadParams[] InitialAdditiveScenes { get; set; }

        #endregion

        #region Mono

        protected override sealed void OnAwake()
        {
            LoadInitialScenes();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void LoadInitialScenes()
        {

        }

        #endregion
    }
}