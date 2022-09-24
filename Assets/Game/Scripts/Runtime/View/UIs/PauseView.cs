using Project.Core;
using Project.Models.Scenes;
using Project.ViewModels;
using Project.ViewModels.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.View.UIs
{
    /// <summary>
    /// Gère le menu pause
    /// </summary>
    public class PauseView : SingletonBehaviour<PauseView>
    {
        #region Public Fields

        [field: SerializeField]
        private GameObject PauseCanvas { get; set; }

        [field: SerializeField]
        private KeyCode[] PauseKeys { get; set; }

        [field: SerializeField]
        private SceneToLoadParams MenuScene { get; set; }

        [field: SerializeField]
        private Slider _volumeSlider { get; set; }

        [field: SerializeField]
        private Slider _mouseSlider { get; set; }

        #endregion


        #region Mono

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

            PauseCanvas.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < PauseKeys.Length; i++)
            {
                if (Input.GetKeyDown(PauseKeys[i]))
                {
                    TogglePause();
                    break;
                }
            }
        }

        #endregion



        #region Public Methods

        public void OnVolumeSliderValueChanged()
        {
            PlayerPrefs.SetFloat("volume", _volumeSlider.value);
        }

        public void OnMouseSliderValueChanged()
        {
            PlayerPrefs.SetFloat("mouse", _mouseSlider.value);
        }

        public void ReturnToMenuBtn()
        {
            TogglePause();
            SceneMaster.Instance.LoadSingleSceneAsync(MenuScene);
        }

        public void TogglePause()
        {
            GameSession.IsGamePaused = !GameSession.IsGamePaused;
            PauseCanvas.SetActive(GameSession.IsGamePaused);
            Time.timeScale = GameSession.IsGamePaused ? 0f : 1f;
        }

        #endregion

    }
}