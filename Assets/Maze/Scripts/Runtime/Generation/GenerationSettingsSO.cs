using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    [CreateAssetMenu(fileName = "New Generation Settings", menuName = "ScriptableObjects/Generation Settings", order = 1)]
    public class GenerationSettingsSO : ScriptableObject
    {

        [field: SerializeField] public DrawMode DrawMode { get; set; } = DrawMode.Console;

        [field: SerializeField] public GenerationType GenerationType { get; set; } = GenerationType.BinaryTree;

        [field: SerializeField] public Vector2Int GridSize { get; set; } = new(10, 10);

        [field: Tooltip("Used in 3D for the width and height of a Cell in the Mesh.")]
        [field: SerializeField] public Vector2Int MeshCellSize { get; set; } = new(5, 5);


        [field: Tooltip("Used by the Recursive Division Algorithm to stop the process if a room is smaller than this size.")]
        [field: SerializeField] public Vector2Int RoomSize { get; set; } = new(5, 5);

        [field: Tooltip("Used by the Recursive Division Algorithm. If true, will create many more rooms than corridors in the maze.")]
        [field: SerializeField] public bool BiasTowardsRooms { get; set; } = false;

        [field: Tooltip("Used by the Growing Tree Algorithm to select different ways to process the unvisited Cells.")]
        [field: SerializeField] public GrowingTreeLambda LambdaSelection { get; set; } = GrowingTreeLambda.RandomCell;


        [field: Tooltip("Generate space between walls. This field is a percentage.")]
        [field: SerializeField, Range(0f, .5f)] public float Inset { get; set; } = 0f;

        [field: Tooltip("Removes deadens to create loops in the maze. This field is a percentage.")]
        [field: SerializeField, Range(0f, 1f)] public float BraidRate { get; set; } = 1f;

        [field: Tooltip("Used by the Houston Algorithm." +
                        "Once the AB alg has visited a number of cells above that percentage, " +
                        "it will use the Wilson Algorithm. This field is a percentage (0 is Wilson & 1 is Aldous-Broder).")]
        [field: SerializeField, Range(0f, 1f)] public float HoustonSwapPercent { get; set; } = .5f;

        [field: Tooltip("The Texture to use for masking the grid's cells.")]
        [field: SerializeField] public Texture2D ImageMask { get; set; }

        [field: Tooltip("The txt file to use for masking the grid's cells.")]
        [field: SerializeField] public TextAsset AsciiMask { get; set; }

        [field: SerializeField] public string Extension { get; set; } = ".png";


#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            //Above 30, we start to see slowdows because of the UI instantiation
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 30), Mathf.Clamp(GridSize.y, 1, 30));
            RoomSize = new(Mathf.Clamp(RoomSize.x, 1, GridSize.x/2), Mathf.Clamp(RoomSize.y, 1, GridSize.y/2));
        }

#endif
    }
}