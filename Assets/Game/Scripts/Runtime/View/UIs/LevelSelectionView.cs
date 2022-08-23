using Project.Models.Game.Enums;
using Project.ViewModels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.View.UIs
{
    public class LevelSelectionView : MonoBehaviour
    {
        //This level will be loaded aynchronously and
        //the ui placed on top of the main menu one.
        //So we just unload this scene.
        public void ReturnToMenuBtn()
        {
            SceneManager.UnloadSceneAsync(1);
        }

        public void LoadEasyModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Easy;
            SceneManager.LoadSceneAsync(3);
        }

        public void LoadNormalModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Normal;
            SceneManager.LoadSceneAsync(3);
        }

        public void LoadHardModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Hard;
            SceneManager.LoadSceneAsync(3);
        }

        public void LoadNightmareModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Nightmare;
            SceneManager.LoadSceneAsync(3);
        }

        public void LoadCustomModeBtn()
        {
            GameSession.DifficultyLevel = Difficulty.Custom;
            SceneManager.LoadSceneAsync(2);
        }
    }
}