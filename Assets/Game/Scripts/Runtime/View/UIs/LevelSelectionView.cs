using Project.Models.Game.Enums;
using Project.ViewModels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.View
{
    public class LevelSelectionView : MonoBehaviour
    {
        public void ReturnToMenuBtn()
        {
            SceneManager.LoadScene(0);
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