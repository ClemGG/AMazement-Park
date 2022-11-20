using Project.ViewModels.Entities.Items;
using Project.ViewModels.Generation;
using TMPro;
using UnityEngine;

namespace Project.View.UIs
{
    /// <summary>
    /// Chargé de màj les icônes du UI de la scène de jeu
    /// </summary>
    public class GameUIView : MonoBehaviour
    {
        #region UI Fields

        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject[] _itemCrosses;
        [SerializeField] private GameObject _mapNoise;
        [SerializeField] private Transform _mapTileContainer;

        #endregion

        #region Private Fields

        private MazeGenerator _generator;
        private Key _key;

        #endregion

        #region Mono

        // Start is called before the first frame update
        void Awake()
        {
            _generator = FindObjectOfType<MazeGenerator>();
            _generator.OnMazeDone += OnMazeDone;
        }

        private void OnDestroy()
        {
            _generator.OnMazeDone -= OnMazeDone;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Quand le dédale est généré, on lie les icones des objets aux ItemTriggers actifs
        /// et la carte au dédale généré dans le MazeGenerator
        /// </summary>
        private void OnMazeDone(object sender, Procedural.MazeGeneration.GenerationProgressReport e)
        {

        }

        #endregion
    }
}