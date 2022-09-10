using System.Collections;
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
        /// Les sc�nes additives � charger quand le SceneMaster est initialis�.
        /// </summary>
        [field: Space(10)]
        [field: SerializeField]
        private SceneToLoadParams[] InitialAdditiveScenes { get; set; }

        #endregion

        #region Mono

        /// <summary>
        /// Quand la sc�ne s'ouvre et cet objet se cr�e,
        /// on charge les sc�ne additives initiales s'il y en a
        /// </summary>
        protected override sealed void OnAwake()
        {
            if(InitialAdditiveScenes.Length > 0)
                LoadAdditiveScenesAsync(InitialAdditiveScenes);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Charge la sc�ne s�lectionn�e en mode Single pour n'avoir qu'une seule sc�ne active
        /// </summary>
        /// <param name="param">La sc�ne � charger</param>
        public void LoadSingleSceneAsync(SceneToLoadParams param)
        {
            SceneManager.LoadSceneAsync(param, LoadSceneMode.Single);
        }

        /// <summary>
        /// Charge la sc�ne s�lectionn�e en mode Single pour n'avoir qu'une seule sc�ne active
        /// </summary>
        /// <param name="param">La sc�ne � charger</param>
        public void UnloadSceneAsync(SceneToLoadParams param)
        {
            SceneManager.UnloadSceneAsync(param);
        }

        /// <summary>
        /// Charge les sc�nes sp�cifi�es asynchronement.
        /// On attend qu'elles soient toutes charg�es avant de les ouvrir
        /// </summary>
        /// <param name="sceneToLoadParams">Les sc�nes � charger</param>
        public void LoadAdditiveScenesAsync(params SceneToLoadParams[] sceneToLoadParams)
        {
            StartCoroutine(LoadAdditiveScenesAsyncCo(sceneToLoadParams));
        }

        /// <summary>
        /// D�charge les sc�nes sp�cifi�es asynchronement.
        /// </summary>
        /// <param name="sceneToLoadParams">Les sc�nes � d�charger</param>
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
        /// Charge toutes les sc�nes asynchronement et ne les ouvre qu'une fois toutes termin�es
        /// </summary>
        private IEnumerator LoadAdditiveScenesAsyncCo(SceneToLoadParams[] sceneToLoadParams)
        {
            AsyncOperation[] operations = new AsyncOperation[sceneToLoadParams.Length];

            for (int i = 0; i < sceneToLoadParams.Length; i++)
            {
                operations[i] = SceneManager.LoadSceneAsync(sceneToLoadParams[i], LoadSceneMode.Additive);
                operations[i].allowSceneActivation = false;
            }

            //On attend que toutes les op�rations soient termin�es
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
                //Une fois le chargement termin�, on les ouvre
                for (int i = 0; i < operations.Length; i++)
                {
                    operations[i].allowSceneActivation = true;

                }

                yield return null;

                //On active la sc�ne marqu�e comme telle dans les param�tres
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