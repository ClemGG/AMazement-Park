using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using UnityEngine.UI;
using Project.Models.Game.Enums;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using static Enums;
using static Project.Services.StringFormatterService;

public class MazeCustomGenerationView : MonoBehaviour
{
    #region Public Fields

    [field: SerializeField, ReadOnly] private CustomMazeSettingsSO Settings { get; set; }
    [field: SerializeField] private bool ShowLongestPaths { get; set; } = false;

    private Project.ViewModels.Generation.MazeGenerator Generator { get; set; }

    #endregion


    #region UI Fields

    //If a field is not represented here, it means it must be assigned the correct values in the Inspector

    [SerializeField] private TMP_Dropdown AlgorithmField;

    [SerializeField] private TMP_InputField GridSizeXField;
    [SerializeField] private TMP_InputField GridSizeYField;

    [SerializeField] private TMP_InputField RoomSizeXField;
    [SerializeField] private TMP_InputField RoomSizeYField;

    [SerializeField] private Toggle BiasTowardsRoomsField;
    [SerializeField] private TMP_Dropdown LambdaSelectionField;
    [SerializeField] private Slider InsetField;
    [SerializeField] private Slider BraidRateField;
    [SerializeField] private Slider HoustonSwapPercentField;

    //Used to modify the slider values directly
    [SerializeField] private TMP_InputField InsetNumberField;
    [SerializeField] private TMP_InputField BraidRateNumberField;
    [SerializeField] private TMP_InputField HoustonSwapPercentNumberField;

    //The mosnter's fields, from to to bottom
    [SerializeField] private Slider Monster1Field;
    [SerializeField] private Slider Monster2Field;
    [SerializeField] private Slider Monster3Field;
    [SerializeField] private Slider Monster4Field;
    [SerializeField] private Slider Monster5Field;

    [SerializeField] private TMP_InputField Monster1NumberField;
    [SerializeField] private TMP_InputField Monster2NumberField;
    [SerializeField] private TMP_InputField Monster3NumberField;
    [SerializeField] private TMP_InputField Monster4NumberField;
    [SerializeField] private TMP_InputField Monster5NumberField;

    //We'll use dropdowns to look for the default assets in the game folder,
    //and give the player an option to browse his computer to add a custom mask.
    //The Toggle is just a preference thing, in case the user wants to use
    //an easier way to draw a mask.
    [SerializeField] private TextMeshProUGUI MaskNameField;
    [SerializeField] private Toggle ShowLongestPathsField;

    #endregion


    #region Private Fields

    //_maskAlgNames contains only the algorithms compatible with masks
    private List<TMP_Dropdown.OptionData> _allAlgOptions = new();
    private List<TMP_Dropdown.OptionData> _maskAlgOptions = new();
    private List<TMP_Dropdown.OptionData> _lambdaOptions = new();
    private readonly string[] _allAlgNames = ValuesToString<GenerationType>().AddSpaces();
    private readonly string[] _maskAlgNames = new string[]
    {
        GenerationType.AldousBroder.ToString().AddSpaces(),
        GenerationType.Eller.ToString().AddSpaces(),
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
            GrowingTreeLambda.Random.ToString().AddSpaces(),
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

    #endregion



    void Start()
    {
        GameSession.DifficultyLevel = Difficulty.Custom;

        InitComponents();
    }

    private void InitComponents()
    {
        //We are in the UI Custom mode, so we need to change the draw mode
        Settings = Resources.Load<CustomMazeSettingsSO>($"Settings/Custom");

        InitSettings();

        Generator = FindObjectOfType<Project.ViewModels.Generation.MazeGenerator>();

        SetAlgorithmOptions();
        OnAlgorithmFieldValueChanged();

        SetLambdaOptions();
        OnLambdaFieldValueChanged();
    }

    //Reset all values to default each time we enter custom mode
    private void InitSettings()
    {
        Settings.DrawMode = DrawMode.UIImage;
        Settings.GenerationType = GenerationType.Random;
        Settings.GridSize = new(10, 10);
        Settings.RoomSize = new(5, 5);
        Settings.BiasTowardsRooms = false;
        Settings.LambdaSelection = GrowingTreeLambda.Random;
        Settings.Inset = 0f;
        Settings.BraidRate = 1f;
        Settings.HoustonSwapPercent = .5f;
        Settings.ImageMask = null;
        Settings.AsciiMask = null;
    }



    //Methods used by the UI elements in the scene
    #region UI Elements


    //Change the algorithm options if the user has entered a mask
    //Also called from the mask buttons
    private void SetAlgorithmOptions()
    {
        AlgorithmField.ClearOptions();

        if (Settings.AsciiMask || Settings.ImageMask)
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
        if (Settings.AsciiMask || Settings.ImageMask)
        {
            algName = MaskAlgOptions[AlgorithmField.value].text.Replace(" ", "");
        }
        else
        {
            algName = AllAlgOptions[AlgorithmField.value].text.Replace(" ", "");
        }

        Settings.GenerationType = GetEnumFromName<GenerationType>(algName);
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
        value = Mathf.Clamp(value, 0f, .35f);
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


    public void PlayMazeBtn()
    {
        SceneManager.LoadSceneAsync("3D scene");
    }

    public void ReturnToMenuBtn()
    {
        SceneManager.LoadSceneAsync("Menu scene");
    }

    public void CancelGenerationBtn()
    {
        Generator.Cleanup();
    }

    public void GenerateMazeBtn()
    {
        Generator.ShowLongestPaths = ShowLongestPaths;
        Generator.Execute(GameSession.DifficultyLevel, DrawMode.UIImage);
    }

    public void OnShowPathsFieldValueChanged()
    {
        ShowLongestPaths = ShowLongestPathsField.isOn;
    }


    #endregion
}
