using System.Collections;
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
        /// Les scènes additives à charger quand le SceneMaster est initialisé.
        /// </summary>
        [field: Space(10)]
        [field: SerializeField]
        private SceneToLoadParams[] InitialAdditiveScenes { get; set; }

        #endregion

        #region Mono

        /// <summary>
        /// Quand la scène s'ouvre et cet objet se crée,
        /// on charge les scène additives initiales s'il y en a
        /// </summary>
        protected override sealed void OnAwake()
        {
            if(InitialAdditiveScenes.Length > 0)
                LoadAdditiveScenesAsync(InitialAdditiveScenes);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Charge la scène sélectionnée en mode Single pour n'avoir qu'une seule scène active
        /// </summary>
        /// <param name="param">La scène à charger</param>
        public void LoadSingleSceneAsync(SceneToLoadParams param)
        {
            SceneManager.LoadSceneAsync(param, LoadSceneMode.Single);
        }

        /// <summary>
        /// Charge la scène sélectionnée en mode Single pour n'avoir qu'une seule scène active
        /// </summary>
        /// <param name="param">La scène à charger</param>
        public void UnloadSceneAsync(SceneToLoadParams param)
        {
            SceneManager.UnloadSceneAsync(param);
        }

        /// <summary>
        /// Charge les scènes spécifiées asynchronement.
        /// On attend qu'elles soient toutes chargées avant de les ouvrir
        /// </summary>
        /// <param name="sceneToLoadParams">Les scènes à charger</param>
        public void LoadAdditiveScenesAsync(params SceneToLoadParams[] sceneToLoadParams)
        {
            StartCoroutine(LoadAdditiveScenesAsyncCo(sceneToLoadParams));
        }

        /// <summary>
        /// Décharge les scènes spécifiées asynchronement.
        /// </summary>
        /// <param name="sceneToLoadParams">Les scènes à décharger</param>
        public void UnloadAdditiveScenesAsync(params SceneToLoadParams[] sceneToLoadParams)
        {
            for (int i = 0; i < sceneToLoadParams.Length; i++)
            {
                UnloadSceneAsync(sceneToLoadParams[i]);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Charge toutes les scènes asynchronement et ne les ouvre qu'une fois toutes terminées
        /// </summary>
        private IEnumerator LoadAdditiveScenesAsyncCo(SceneToLoadParams[] sceneToLoadParams)
        {
            AsyncOperation[] operations = new AsyncOperation[sceneToLoadParams.Length];

            for (int i = 0; i < sceneToLoadParams.Length; i++)
            {
                operations[i] = SceneManager.LoadSceneAsync(sceneToLoadParams[i], LoadSceneMode.Additive);
                operations[i].allowSceneActivation = false;
            }

            //On attend que toutes les opérations soient terminées
            bool loadingDone = false;
            do
            {
                loadingDone = true;
                for (int i = 0; i < operations.Length; i++)
                {
                    if (operations[i].progress < .9f)
                    {
                        loadingDone = false;
                        break;
                    }
                }
                yield return null;
            } while (!loadingDone);


            if (loadingDone)
            {
                //Une fois le chargement terminé, on les ouvre
                for (int i = 0; i < operations.Length; i++)
                {
                    operations[i].allowSceneActivation = true;

                }

                yield return null;

                //On active la scène marquée comme telle dans les paramètres
                for (int i = 0; i < sceneToLoadParams.Length; i++)
                {
                    if (sceneToLoadParams[i].SetAsActiveScene)
                    {
                        Scene newActiveScene = SceneManager.GetSceneByName(sceneToLoadParams[i]);
                        if (SceneManager.GetActiveScene() != newActiveScene)
                        {
                            SceneManager.SetActiveScene(newActiveScene);
                        }
                        break;
                    }

                }
            }
        }

        #endregion
    }
}