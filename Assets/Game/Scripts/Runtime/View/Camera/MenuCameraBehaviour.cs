using UnityEngine;

using Project.ViewModels.Maze;
using System;
using UnityEngine.AI;

namespace Project.View.Camera
{
    //The Camera in the Main Menu just picks a random Cell
    //in the generated maze and moves towards that destination.
    public class MenuCameraBehaviour : MonoBehaviour
    {
        private NavMeshAgent Agent { get; set; }
        private NavMeshSurface

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            gameObject.SetActive(false);
        }

        //Called when the OnMazeDone event is raised in the MazeGenerator
        private void Start()
        {
            SetupNavMesh();
            StartRandomMovement();
        }

        private void SetupNavMesh()
        {

        }

        private void StartRandomMovement()
        {
            throw new NotImplementedException();
        }
    }
}