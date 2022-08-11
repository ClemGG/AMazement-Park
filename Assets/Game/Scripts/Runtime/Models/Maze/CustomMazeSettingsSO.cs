using UnityEngine;
using Project.Procedural.MazeGeneration;
using Project.Models.Game.Enums;

namespace Project.Models.Maze
{
    [CreateAssetMenu(fileName = "New Custom Maze Settings", menuName = "ScriptableObjects/Custom Maze Settings (Use this)", order = 1)]
    public class CustomMazeSettingsSO : GenerationSettingsSO
    {
        [field: SerializeField] public Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;
        [field: SerializeField] public string AsciiMaskName { get; set; }

    }
}