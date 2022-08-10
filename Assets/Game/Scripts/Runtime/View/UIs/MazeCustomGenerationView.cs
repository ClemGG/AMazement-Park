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

    [SerializeField] private Toggle BiasTwardsRoomsField;
    [SerializeField] private Slider LambdaSelectionField;
    [SerializeField] private Slider InsetField;
    [SerializeField] private Slider BraidRateField;
    [SerializeField] private Slider HoustonSwapPercentField;

    //Used to modify the slider values directly
    [SerializeField] private TMP_InputField LambdaSelectionNumberField;
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

    #endregion


    #region Private Fields

    //_maskAlgNames contains only the algorithms compatible with masks
    private List<TMP_Dropdown.OptionData> _allAlgOptions = new();
    private List<TMP_Dropdown.OptionData> _maskAlgOptions = new();
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

    #endregion

    void Start()
    {
        GameSession.DifficultyLevel = Difficulty.Custom;

        GetComponents();
    }

    private void GetComponents()
    {
        //We are in the UI Custom mode, so we need to change the draw mode
        Settings = Resources.Load<CustomMazeSettingsSO>($"Settings/Custom");
        Settings.DrawMode = DrawMode.UIImage;

        Generator = FindObjectOfType<Project.ViewModels.Generation.MazeGenerator>();

        SetAlgorithmOptions();
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


    #endregion
}
