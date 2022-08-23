using UnityEngine;

using Project.ViewModels.Maze;
using UnityEngine.AI;

namespace Project.View.Camera
{
    //The Camera in the Main Menu just picks a random Cell
    //in the generated maze and moves towards that destination.
    public class MenuCameraBehaviour : MonoBehaviour
    {
        private NavMeshAgent Agent { get; set; }
        private NavMeshSurface FloorNavMesh { get; set; }


        //Called when the OnMazeDone event is raised in the MazeGenerator
        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            FloorNavMesh = FindObjectOfType<NavMeshSurface>();


            SetupNavMesh();
            StartRandomMovement();
        }

        private void SetupNavMesh()
        {

        }

        private void StartRandomMovement()
        {

        }
    }
}