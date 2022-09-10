using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Models.Scenes
{
    /// <summary>
    /// Contient les param�tres des sc�nes � charger par le SceneMaster.
    /// Peut �tre pass� en param�tre � SceneMaster.LoadScenes() et UnloadScenes()
    /// pour (d�)charger une sc�ne sp�cifique.
    /// </summary>
    [System.Serializable]
    public class SceneToLoadParams
    {
        /// <summary>
        /// La sc�ne � charger.
        /// </summary>
        [field: SerializeField]
        public Object SceneToLoad { get; set; }

        /// <summary>
        /// Si � SINGLE, cette sc�ne �crasera toutes les autres.
        /// Si � ADDITIVE, elle s'ajoutera � la suite des autres.
        /// </summary>
        [field: SerializeField]
        public LoadSceneMode LoadSceneMode { get; set; } = LoadSceneMode.Additive;

        /// <summary>
        /// Peut-il y avoir plusieurs copies de cette sc�ne � la fois ?
        /// </summary>
        [field: SerializeField]
        public bool AllowDuplicates { get; set; } = false;

        /// <summary>
        /// Si � TRUE, cette sc�ne doit �tre la sc�ne active (principale).
        /// </summary>
        [field: SerializeField]
        public bool SetAsActiveScene { get; set; } = false;


        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneToLoadParams sceneParams)
        {
            return sceneParams.SceneToLoad.name;
        }
    }
}