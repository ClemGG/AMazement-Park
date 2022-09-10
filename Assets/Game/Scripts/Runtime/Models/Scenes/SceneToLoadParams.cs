using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Models.Scenes
{
    /// <summary>
    /// Contient les paramètres des scènes à charger par le SceneMaster.
    /// Peut être passé en paramètre à SceneMaster.LoadScenes() et UnloadScenes()
    /// pour (dé)charger une scène spécifique.
    /// </summary>
    [System.Serializable]
    public class SceneToLoadParams
    {
        /// <summary>
        /// La scène à charger.
        /// </summary>
        [field: SerializeField]
        public Object SceneToLoad { get; set; }

        /// <summary>
        /// Si à SINGLE, cette scène écrasera toutes les autres.
        /// Si à ADDITIVE, elle s'ajoutera à la suite des autres.
        /// </summary>
        [field: SerializeField]
        public LoadSceneMode LoadSceneMode { get; set; } = LoadSceneMode.Additive;

        /// <summary>
        /// Peut-il y avoir plusieurs copies de cette scène à la fois ?
        /// </summary>
        [field: SerializeField]
        public bool AllowDuplicates { get; set; } = false;

        /// <summary>
        /// Si à TRUE, cette scène doit être la scène active (principale).
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