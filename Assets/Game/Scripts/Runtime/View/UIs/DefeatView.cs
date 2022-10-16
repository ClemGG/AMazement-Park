using Project.Models.Scenes;
using Project.ViewModels.Scenes;
using UnityEngine;

namespace Project.View.UIs
{
    /// <summary>
    /// Affiche quand le joueur perd la partie
    /// </summary>
    public class DefeatView : MonoBehaviour
    {
        #region Public Fields

        [field: SerializeField]
        public SceneToLoadParams MenuScene { get; set; }

        #endregion

        #region Mono

        // Start is called before the first frame update
        void Start()
        {
            //TODO : Afficher l'animation de mort avant le UI
        }

        #endregion

        #region Public Fields

        public void ReturnToMenuBtn()
        {
            SceneMaster.Instance.LoadSingleSceneAsync(MenuScene);
        }

        #endregion

    }
}