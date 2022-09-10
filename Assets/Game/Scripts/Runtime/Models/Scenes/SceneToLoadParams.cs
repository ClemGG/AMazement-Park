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