using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using UnityEngine.UI;
using Project.Models.Game.Enums;

public class MazeCustomGenerationView : MonoBehaviour
{
    #region Public Fields

    [field: SerializeField, ReadOnly] private CustomMazeSettingsSO Settings { get; set; }
    [field: SerializeField] private bool ShowLongestPaths { get; set; } = false;

    #endregion


    #region UI Fields

    //If a field is not represented here, it means it must be assigned the correct values in the Inspector

    private Dropdown GenerationTypeField;

    private InputField GridSizeXField;
    private InputField GridSizeYField;

    private InputField RoomSizeXField;
    private InputField RoomSizeYField;

    private Toggle BiasTwardsRoomsField;
    private Slider LambdaSelectionField;
    private Slider InsetField;
    private Slider BraidRateField;
    private Slider HoustonSwapPercentField;

    //We'll use dropdowns to look for the default assets in the game folder,
    //and give the player an option to browse his computer to add a custom mask.
    //The Toggle is just a preference thing, in case the user wants to use
    //an easier way to draw a mask.
    private Dropdown ImageMaskField;
    private Dropdown ASCIIMaskField;
    private Toggle UseTextMaskInstead;


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
    }



    //Methods used by the UI elements in the scene
    #region UI Elements

    public void GenerateMazeBtn()
    {
        var generator = FindObjectOfType<Project.ViewModels.Generation.MazeGenerator>();
        generator.ShowLongestPaths = ShowLongestPaths;
        generator.Execute(GameSession.DifficultyLevel, DrawMode.UIImage);
    }


    #endregion
}
