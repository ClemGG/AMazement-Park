using Project.Models.Game.Enums;
using Project.Models.Scenes;
using Project.ViewModels;
using Project.ViewModels.Scenes;
using UnityEngine;

namespace Project.View.UIs
{
    public class LevelSelectionView : MonoBehaviour
    {
        #region Public Fields

        [field: SerializeField]
        private SceneToLoadParams LevelSelectScene { get; set; }
        [field: SerializeField]
        private SceneToLoadParams GameScene { get; set; }
        [field: SerializeField]
        private SceneToLoadParams CustomModeScene { get; set; }

        #endregion

        #region Public Methods

        //This level will be loaded aynchronously and
        //the ui placed on top of the main menu one.
        //So we just unload this scene.
        public void ReturnToMenuBtn()
        {
            SceneMaster.Instance.UnloadSceneAsync(LevelSelectScene);
        }

        public void LoadEasyModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Easy;
            SceneMaster.Instance.LoadSingleSceneAsync(GameScene);
        }

        public void LoadNormalModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Normal;
            SceneMaster.Instance.LoadSingleSceneAsync(GameScene);
        }

        public void LoadHardModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Hard;
            SceneMaster.Instance.LoadSingleSceneAsync(GameScene);
        }

        public void LoadNightmareModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Nightmare;
            SceneMaster.Instance.LoadSingleSceneAsync(GameScene);
        }

        public void LoadCustomModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Custom;
            SceneMaster.Instance.LoadSingleSceneAsync(CustomModeScene);
        }

        #endregion
    }
}