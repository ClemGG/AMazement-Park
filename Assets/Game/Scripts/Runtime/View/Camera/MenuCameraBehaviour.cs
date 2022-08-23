using UnityEngine;

using Project.ViewModels.Maze;
using UnityEngine.AI;
using Project.Models.Maze;

namespace Project.View.Camera
{
    //The Camera in the Main Menu just picks a random Cell
    //in the generated maze and moves towards that destination.
    public class MenuCameraBehaviour : MonoBehaviour
    {
        //small offset to not get stuck to walls
        [field: SerializeField] public float RadiusToWallOffset { get; set; } = .25f;
        [field: SerializeField] public CustomMazeSettingsSO Settings { get; set; }
        private NavMeshAgent Agent { get; set; }
        private NavMeshSurface FloorNavMesh { get; set; }

        #region Mono

        //Called when the OnMazeDone event is raised in the MazeGenerator
        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            FloorNavMesh = FindObjectOfType<NavMeshSurface>();


            SetupNavMesh();
            StartRandomMovement();
        }

        private void Update()
        {
            
        }

        #endregion

        #region AI

        private void SetupNavMesh()
        {
            //The cam's AI radius takes half the entire cell if no inset,
            //or a max. of a quarter of the cell if Inset is at max value (.25f)
            //This will give us an AI going roughly through the center of each cell

            Agent.radius = Settings.MeshCellSize.x / Mathf.Lerp(2f, 4f, Settings.Inset / .25f);
            Agent.radius -= RadiusToWallOffset; 

            FloorNavMesh.BuildNavMesh();
        }

        private void StartRandomMovement()
        {

        }

        #endregion
    }
}