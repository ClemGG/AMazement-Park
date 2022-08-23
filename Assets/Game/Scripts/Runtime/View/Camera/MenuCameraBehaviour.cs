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

        private Vector3 _lastPosition;
        private Vector3 _nextPosition;
        private bool _isNavigating = false;

        #region Mono

        //Called when the OnMazeDone event is raised in the MazeGenerator
        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            FloorNavMesh = FindObjectOfType<NavMeshSurface>();


            SetupNavMesh();
        }

        private void Update()
        {
            if (!_isNavigating)
            {
                GetNextDestination();
                _isNavigating = true;
            }

            //Once he has reached the designated cell,
            //we tell him to search for another one.
            if(Agent.remainingDistance < Agent.stoppingDistance)
            {
                _isNavigating = false;
            }
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

            //Make sure the NavMeshSurface has been baked previously
            //in edit mode so that it binds the agent to it.
            FloorNavMesh.BuildNavMesh();
        }

        private void GetNextDestination()
        {
            Vector3 next;
            do
            {
                next = MazeManager.GetRandomPosition() * Settings.MeshCellSize.x;
            } 
            //We keep randomizing the position while the next pos is too close to the old one.
            while (next == _nextPosition && (_lastPosition - next).sqrMagnitude < 25f);

            _nextPosition = next;

            //Makes sure the agent doesn't start above the air
            if (_lastPosition == Vector3.zero)
            {
                Agent.Warp(_nextPosition);
            }

            Agent.SetDestination(_nextPosition);
            _lastPosition = _nextPosition;
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_nextPosition, 5f);
        }

#endif
    }
}