using Project.Models.Game.Enums;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.View.UIs
{
    public class MainMenuView : MonoBehaviour
    {
        [field: SerializeField] private GameObject MenuCam { get; set; }
        
        private ViewModels.Generation.MazeGenerator Generator { get; set; }

        #region UI Fields

        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Slider _mouseSlider;

        #endregion

        private void Awake()
        {
            MenuCam.SetActive(false);
            Generator = FindObjectOfType<ViewModels.Generation.MazeGenerator>();
            Generator.OnMazeDone += OnMazeDone;

            //Generates a randome maze for the Menu Cam to roam in
            Generator.Execute(Difficulty.Random, DrawMode.Mesh);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!PlayerPrefs.HasKey("volume"))
            {
                PlayerPrefs.SetFloat("volume", .3f);
            }
            if (!PlayerPrefs.HasKey("mouse"))
            {
                PlayerPrefs.SetFloat("mouse", .5f);
            }

            _volumeSlider.value = PlayerPrefs.GetFloat("volume", .3f);
            _mouseSlider.value = PlayerPrefs.GetFloat("mouse", .5f);
        }



        //We start the camera's movement process across the maze once it's complete
        private void OnMazeDone(object sender, GenerationProgressReport e)
        {
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                MenuCam.SetActive(true);
                Generator.OnMazeDone -= OnMazeDone;
            }
        }


        #region UI Fields

        public void OnVolumeSliderValueChanged()
        {
            PlayerPrefs.SetFloat("volume", _volumeSlider.value);
        }
        public void OnMouseSliderValueChanged()
        {
            PlayerPrefs.SetFloat("mouse", _mouseSlider.value);
        }

        public void PlayBtn()
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        public void QuitBtn()
        {
            Application.Quit();
        }


        #endregion
    }
}