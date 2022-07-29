using Project.Models.Maze;
using Project.Procedural.MazeGeneration;
using UnityEngine;
using Project.Models.Game.Enums;

public class MazeCustomGenerationView : MonoBehaviour
{
    [field : SerializeField, ReadOnly] private CustomMazeSettingsSO Settings { get; set; }
    [field: SerializeField] private bool ShowLongestPaths { get; set; } = false;


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
