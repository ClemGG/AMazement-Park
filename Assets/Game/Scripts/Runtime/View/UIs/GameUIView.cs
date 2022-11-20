using TMPro;
using UnityEngine;

namespace Project.View.UIs
{
    /// <summary>
    /// Charg� de m�j les ic�nes du UI de la sc�ne de jeu
    /// </summary>
    public class GameUIView : MonoBehaviour
    {
        #region UI Fields

        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject[] _itemCrosses;
        [SerializeField] private GameObject _mapNoise;
        [SerializeField] private Transform _mapTileContainer;

        #endregion

        #region Mono

        // Start is called before the first frame update
        void Start()
        {

        }

        #endregion
    }
}