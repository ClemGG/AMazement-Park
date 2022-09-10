using Project.Core;
using Project.Models.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.ViewModels.Scenes
{
    /// <summary>
    /// G�re le changement de sc�nes et le chargement de sc�nes en additif
    /// </summary>
    public class SceneMaster : SingletonBehaviour<SceneMaster>
    {
        #region Public Fields


        /// <summary>
        /// La premi�re sc�ne active quand le SceneMaster est initialis�.
        /// </summary>
        [field: Space(10)]
        [field: Header("Scene Parameters")]
        [field: SerializeField]
        private SceneToLoadParams InitialFirstActiveScene { get; set; }

        /// <summary>
        /// Les sc�nes additives � charger quand le SceneMaster est initialis�.
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