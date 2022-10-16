using System.Collections.Generic;
using System.IO;
using Project.Models.Game.Enums;
using Project.Models.Maze;
using Project.Models.Scenes;
using Project.Procedural.MazeGeneration;
using Project.ViewModels;
using Project.ViewModels.Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using static Project.Services.StringFormatterService;

namespace Project.View.UIs
{
    public class MazeCustomGenerationView : MonoBehaviour
    {
        #region Public Fields

        [field: SerializeField, ReadOnly] private CustomMazeSettingsSO Settings { get; set; }
        [field: SerializeField] private bool ShowBestPaths { get; set; } = false;
        [field: SerializeField] private bool ShowHeatMap { get; set; } = false;
        [field: SerializeField] private Gradient MonsteJaugeGradient { get; set; }
        [field: SerializeField] private SceneToLoadParams GameScene { get; set; }
        [field: SerializeField] private SceneToLoadParams MainMenuScene { get; set; }
        [field: Space(10)]

        #endregion


        #region UI Fields

        //If a field is not represented here, it means it must be assigned the correct values in the Inspector

        [SerializeField] private GameObject Slot0;  //Algorithm
        [SerializeField] private GameObject Slot1;  //Grid Size
        [SerializeField] private GameObject Slot2;  //Room Size
        [SerializeField] private GameObject Slot3;  //Bias
        [SerializeField] private GameObject Slot4;  //Lambda
        [SerializeField] private GameObject Slot5;  //Inset
        [SerializeField] private GameObject Slot6;  //Braid
        [SerializeField] private GameObject Slot7;  //Houston
        [SerializeField] private GameObject Slot8;  //Mask

        [Space(10)]

        [SerializeField] private TMP_Dropdown AlgorithmField;

        [SerializeField] private TMP_InputField GridSizeXField;
        [SerializeField] private TMP_InputField GridSizeYField;

        [SerializeField] private TMP_InputField RoomSizeXField;
        [SerializeField] private TMP_InputField RoomSizeYField;

        [Space(10)]

        [SerializeField] private Toggle BiasTowardsRoomsField;
        [SerializeField] private TMP_Dropdown LambdaSelectionField;
        [SerializeField] private Slider InsetField;
        [SerializeField] private Slider BraidRateField;
        [SerializeField] private Slider HoustonSwapPercentField;
        [SerializeField] private Slider PlayerFOVField;

        [Space(10)]

        //Used to modify the slider values directly
        [SerializeField] private TMP_InputField InsetNumberField;
        [SerializeField] private TMP_InputField BraidRateNumberField;
        [SerializeField] private TMP_InputField HoustonSwapPercentNumberField;
        [SerializeField] private TMP_InputField PlayerFOVNumberField;
        [SerializeField] private TMP_InputField MaskField;

        [Space(10)]

        //The mosnter's fields, from to to bottom

        [SerializeField] private Slider[] MonsterFields;

        [SerializeField] private Transform[] MonsterFillJauges;
        [SerializeField] private GameObject[] ItemCrosses;
        [SerializeField] private GameObject[] MonsterCrosses;

        [Space(10)]

        //We'll use dropdowns to look for the default assets in the game folder,
        //and give the player an option to browse his computer to add a custom mask.
        //The Toggle is just a preference thing, in case the user wants to use
        //an easier way to draw a mask.
        [SerializeField] private TextMeshProUGUI MaskLabel;
        [SerializeField] private Toggle ShowLongestPathsField;
        [SerializeField] private Toggle ShowHeatMapField;

        private static GameObject _progressCanvas;
        private static GameObject ProgressCanvas
        {
            get
            {
                if (!_progressCanvas) _progressCanvas = GameObject.Find("Progress canvas");
                return _progressCanvas;
            }
        }

        #endregion


        #region Private Fields

        private ViewModels.Generation.MazeGenerator Generator { get; set; }

        //_maskAlgNames contains only the algorithms compatible with masks
        private List<TMP_Dropdown.OptionData> _allAlgOptions = new();
        private List<TMP_Dropdown.OptionData> _maskAlgOptions = new();
        private List<TMP_Dropdown.OptionData> _lambdaOptions = new();
        private readonly string[] _allAlgNames = ValuesToString<GenerationType>().AddSpaces();
        private readonly string[] _maskAlgNames = new string[]
        {
        GenerationType.AldousBroder.ToString().AddSpaces(),
        GenerationType.GrowingTree.ToString().AddSpaces(),
        GenerationType.Houston.ToString().AddSpaces(),
        GenerationType.HuntAndKill.ToString().AddSpaces(),
        GenerationType.RandomizedKruskal.ToString().AddSpaces(),
        GenerationType.RecursiveBacktracker.ToString().AddSpaces(),
        GenerationType.SimplifiedPrim.ToString().AddSpaces(),
        GenerationType.TruePrim.ToString().AddSpaces(),
        GenerationType.Wilson.ToString().AddSpaces(),
        };
        private readonly string[] _lambdaNames = new string[]
        {
            GrowingTreeLambda.RandomCell.ToString().AddSpaces(),
            GrowingTreeLambda.LastCell.ToString().AddSpaces(),
            GrowingTreeLambda.FirstCell.ToString().AddSpaces(),
            GrowingTreeLambda.FirstAndLastMix.ToString().AddSpaces(),
        };

        private List<TMP_Dropdown.OptionData> AllAlgOptions
        {
            get
            {
                if (_allAlgOptions.Count == 0)
                {
                    for (int i = 0; i < _allAlgNames.Length; i++)
                    {
                        _allAlgOptions.Add(new(_allAlgNames[i]));
                    }
                }

                return _allAlgOptions;
            }
        }
        private List<TMP_Dropdown.OptionData> MaskAlgOptions
        {
            get
            {
                if (_maskAlgOptions.Count == 0)
                {
                    for (int i = 0; i < _maskAlgNames.Length; i++)
                    {
                        _maskAlgOptions.Add(new(_maskAlgNames[i]));
                    }
                }

                return _maskAlgOptions;
            }
        }
        private List<TMP_Dropdown.OptionData> LambdaOptions
        {
            get
            {
                if (_lambdaOptions.Count == 0)
                {
                    for (int i = 0; i < _lambdaNames.Length; i++)
                    {
                        _lambdaOptions.Add(new(_lambdaNames[i]));
                    }
                }

                return _lambdaOptions;
            }
        }


        private List<string> _allowedExtensions = new() { ".txt", ".png", ".jpg", ".jpeg", ".jpe", ".jfif" };


        #endregion


        #region Init

        void Start()
        {
            InitComponents();
        }

        private void InitComponents()
        {
            //We are in the UI Custom mode, so we need to change the draw mode
            Settings = Resources.Load<CustomMazeSettingsSO>($"Settings/Custom");
            InitSettings();
            GameSession.DifficultyLevel = Difficulty.Custom;



            Generator = FindObjectOfType<ViewModels.Generation.MazeGenerator>();
            ProgressCanvas.SetActive(false);


            SetAlgorithmOptions();
            OnAlgorithmFieldValueChanged();

            SetLambdaOptions();
            OnLambdaFieldValueChanged();

            //Init monster jauges
            for (int i = 0; i < MonsterFillJauges.Length; i++)
            {
                UpdateMonsterJauges(i);
            }
        }

        //Reset all values to default each time we enter custom mode
        private void InitSettings()
        {
            Settings.DrawMode = DrawMode.UIImage;
            Settings.GenerationType = GenerationType.Random;
            Settings.GridSize = new(10, 10);
            Settings.RoomSize = new(5, 5);
            Settings.BiasTowardsRooms = false;
            Settings.LambdaSelection = GrowingTreeLambda.RandomCell;
            Settings.Inset = 0f;
            Settings.BraidRate = 1f;
            Settings.HoustonSwapPercent = .5f;
            Settings.ImageMask = null;
            Settings.AsciiMask = null;
            Settings.MaskName = "";
            Settings.ActivityLevels = new int[5] { 3, 3, 3, 3, 2 };
            Settings.ActiveItems = new bool[3] { true, true, true };
            Settings.PlayerFOV = 7f;
            GameSession.ActivityLevels = new int[5] { 2, 2, 2, 2, 2 };
        }

        #endregion


        //Methods used by the UI elements in the scene
        #region UI Elements


        //Change the algorithm options if the user has entered a mask
        //Also called from the mask buttons
        private void SetAlgorithmOptions()
        {
            AlgorithmField.ClearOptions();

            if (Settings.MaskName != "")
            {
                AlgorithmField.AddOptions(MaskAlgOptions);
            }
            else
            {
                AlgorithmField.AddOptions(AllAlgOptions);
            }
        }
        public void OnAlgorithmFieldValueChanged()
        {
            string algName = "";
            if (Settings.MaskName != "")
            {
                algName = MaskAlgOptions[AlgorithmField.value].text.Replace(" ", "");
            }
            else
            {
                algName = AllAlgOptions[AlgorithmField.value].text.Replace(" ", "");
            }

            Settings.GenerationType = GetEnumFromName<GenerationType>(algName);
            ShowAndHideSlots();
        }

        //Hides the sections of the UI irrelevant to the current algorithm
        private void ShowAndHideSlots()
        {
            Slot0.SetActive(true);
            Slot1.SetActive(true);
            Slot2.SetActive(true);
            Slot3.SetActive(true);
            Slot4.SetActive(true);
            Slot5.SetActive(true);
            Slot6.SetActive(true);
            Slot7.SetActive(true);
            Slot8.SetActive(true);

            switch (Settings.GenerationType)
            {
                case
                GenerationType.BinaryTree or
                GenerationType.Sidewinder or
                GenerationType.Eller:
                    Slot2.SetActive(false);
                    Slot3.SetActive(false);
                    Slot4.SetActive(false);
                    Slot7.SetActive(false);
                    Slot8.SetActive(false);
                    break;
                case
                GenerationType.AldousBroder or
                GenerationType.Wilson or
                GenerationType.HuntAndKill or
                GenerationType.RecursiveBacktracker or
                GenerationType.RandomizedKruskal or
                GenerationType.SimplifiedPrim or
                GenerationType.TruePrim:
                    Slot2.SetActive(false);
                    Slot3.SetActive(false);
                    Slot4.SetActive(false);
                    Slot7.SetActive(false);
                    break;

                case GenerationType.Houston:
                    Slot2.SetActive(false);
                    Slot3.SetActive(false);
                    Slot4.SetActive(false);
                    break;

                case GenerationType.GrowingTree:
                    Slot2.SetActive(false);
                    Slot3.SetActive(false);
                    Slot7.SetActive(false);
                    break;

                case GenerationType.RecursiveDivision:
                    Slot4.SetActive(false);
                    Slot7.SetActive(false);
                    Slot8.SetActive(false);
                    break;

                case GenerationType.OneRoom:
                    Slot2.SetActive(false);
                    Slot3.SetActive(false);
                    Slot4.SetActive(false);
                    Slot5.SetActive(false);
                    Slot6.SetActive(false);
                    Slot7.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void OnGridSizeXFieldEndEdit()
        {
            int value = int.Parse(GridSizeXField.text);
            value = Mathf.Clamp(value, 10, 30);
            GridSizeXField.text = value.ToString();

            Settings.GridSize = new(value, Settings.GridSize.y);
        }
        public void OnGridSizeYFieldEndEdit()
        {
            int value = int.Parse(GridSizeYField.text);
            value = Mathf.Clamp(value, 10, 30);
            GridSizeYField.text = value.ToString();

            Settings.GridSize = new(Settings.GridSize.x, value);
        }


        public void OnRoomSizeXFieldEndEdit()
        {
            int value = int.Parse(RoomSizeXField.text);
            value = Mathf.Clamp(value, 1, 30);
            RoomSizeXField.text = value.ToString();

            Settings.RoomSize = new(value, Settings.RoomSize.y);
        }
        public void OnRoomSizeYFieldEndEdit()
        {
            int value = int.Parse(RoomSizeYField.text);
            value = Mathf.Clamp(value, 1, 30);
            RoomSizeYField.text = value.ToString();

            Settings.RoomSize = new(Settings.RoomSize.x, value);
        }
        public void OnBiasFieldValueChanged()
        {
            Settings.BiasTowardsRooms = BiasTowardsRoomsField.isOn;
        }


        private void SetLambdaOptions()
        {
            LambdaSelectionField.ClearOptions();
            LambdaSelectionField.AddOptions(LambdaOptions);
        }
        public void OnLambdaFieldValueChanged()
        {
            string algName = LambdaSelectionField.captionText.text.Replace(" ", "");
            Settings.LambdaSelection = GetEnumFromName<GrowingTreeLambda>(algName);
        }

        public void OnInsetFieldValueChanged()
        {
            Settings.Inset = InsetField.value;
            InsetNumberField.text = $"{InsetField.value:0.00}";
        }
        public void OnInsetNumberFieldEndEdit()
        {
            float value = float.Parse(InsetNumberField.text);
            value = Mathf.Clamp(value, 0f, .25f);
            InsetNumberField.text = value.ToString();
            InsetField.value = value;

            Settings.Inset = value;
        }

        public void OnBraidRateFieldValueChanged()
        {
            Settings.BraidRate = BraidRateField.value;
            BraidRateNumberField.text = $"{BraidRateField.value:0.00}";
        }
        public void OnBraidRateNumberFieldEndEdit()
        {
            float value = float.Parse(BraidRateNumberField.text);
            value = Mathf.Clamp(value, 0f, 1f);
            BraidRateNumberField.text = value.ToString();
            BraidRateField.value = value;

            Settings.BraidRate = value;
        }

        public void OnHoustonFieldValueChanged()
        {
            Settings.HoustonSwapPercent = HoustonSwapPercentField.value;
            HoustonSwapPercentNumberField.text = $"{HoustonSwapPercentField.value:0.00}";
        }
        public void OnHoustonNumberFieldEndEdit()
        {
            float value = float.Parse(HoustonSwapPercentNumberField.text);
            value = Mathf.Clamp(value, 0f, 1f);
            HoustonSwapPercentNumberField.text = value.ToString();
            HoustonSwapPercentField.value = value;

            Settings.HoustonSwapPercent = value;
        }

        public void BrowseBtn()
        {
            string path = $"{Application.streamingAssetsPath}/Masks/{MaskField.text}";
            string extension = Path.GetExtension(path);
            if (!File.Exists(path) || !_allowedExtensions.Contains(extension))
            {
                print(File.Exists(path) + " " + _allowedExtensions.Contains(extension));
                MaskLabel.text = "<color=#ff0000>Wrong file name or extension</color>";
            }
            else
            {
                Settings.MaskName = MaskField.text.Replace(extension, "");
                Settings.Extension = extension;
                MaskLabel.text = "<color=#00ff00>Mask loaded!</color>";

                SetAlgorithmOptions();
                OnAlgorithmFieldValueChanged();
            }
        }
        public void ClearMaskBtn()
        {
            MaskLabel.text = "<color=#ffffff>Mask: None</color>";
            MaskField.text = "";
            Settings.ImageMask = null;
            Settings.AsciiMask = null;
            Settings.MaskName = "";

            SetAlgorithmOptions();
            OnAlgorithmFieldValueChanged();
        }

        public void OnMonsterFieldValueChanged(int index)
        {
            int value = (int)MonsterFields[index].value;
            //Si c'est le Chasseur, on ne met au minimum au niveau 2
            //pour ne jamais le désactiver
            if(index == 0)
            {
                value = Mathf.Max(value, 2);
                MonsterFields[index].value = value;
            }
            Settings.ActivityLevels[index] = value;
            UpdateMonsterJauges(index);
        }
        private void UpdateMonsterJauges(int index)
        {
            Transform jauge = MonsterFillJauges[index];
            int value = (int)MonsterFields[index].value;
            for (int i = 0; i < jauge.childCount; i++)
            {
                Image img = jauge.GetChild(i).GetComponent<Image>();
                img.gameObject.SetActive(i < value);
                img.color = MonsteJaugeGradient.Evaluate((i + 1) / 5f);
            }

            MonsterCrosses[index].gameObject.SetActive(value == 1);
        }

        public void EnableItemBtn(int index)
        {
            Settings.ActiveItems[index] = !Settings.ActiveItems[index];
            ItemCrosses[index].SetActive(!ItemCrosses[index].activeSelf);
        }

        public void OnPlayerFOVFieldValueChanged()
        {
            Settings.PlayerFOV = PlayerFOVField.value * Settings.MeshCellSize.x;
            PlayerFOVNumberField.text = $"{PlayerFOVField.value:0}";
        }
        public void OnPlayerFOVNumberFieldEndEdit()
        {
            float value = float.Parse(PlayerFOVNumberField.text);
            value = Mathf.Clamp(value, 3f, 9f);
            PlayerFOVNumberField.text = value.ToString();
            PlayerFOVField.value = value;

            Settings.PlayerFOV = value * Settings.MeshCellSize.x;
        }



        public void PlayMazeBtn()
        {
            Generator.Cleanup();
            GameSession.ResetGameSession();
            SceneMaster.Instance.LoadSingleSceneAsync(GameScene);
        }

        public void ReturnToMenuBtn()
        {
            Generator.Cleanup();
            SceneMaster.Instance.LoadSingleSceneAsync(MainMenuScene);
        }

        public void CancelGenerationBtn()
        {
            Generator.Cleanup();
        }

        public void GenerateMazeBtn()
        {
            Generator.ShowBestPaths = ShowBestPaths;
            Generator.ShowHeatMap = ShowHeatMap;

            ProgressCanvas.SetActive(true);
            Generator.Execute(Difficulty.Custom, DrawMode.UIImage);
        }

        public void OnShowPathsFieldValueChanged()
        {
            ShowBestPaths = ShowLongestPathsField.isOn;
        }
        public void OnShowHeatMapFieldValueChanged()
        {
            ShowHeatMap = ShowHeatMapField.isOn;
        }


        #endregion
    }
}