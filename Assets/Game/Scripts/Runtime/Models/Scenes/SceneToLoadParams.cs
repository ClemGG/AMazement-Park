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