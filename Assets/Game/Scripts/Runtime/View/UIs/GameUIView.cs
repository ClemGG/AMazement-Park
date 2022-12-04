using System.Collections;
using System.Collections.Generic;
using Project.Models.Game.Enums;
using Project.ViewModels;
using Project.ViewModels.Entities;
using Project.ViewModels.Entities.EventArgs;
using Project.ViewModels.Entities.Items;
using Project.ViewModels.Generation;
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

        [Header("Compponents :")]
        [SerializeField] private MapNoise _mapNoise;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject[] _itemCrosses;
        [SerializeField] private GameObject _mapNoiseGo;
        [SerializeField] private Transform _mapTileContainer;

        [Header("Settings :")]
        [SerializeField] private Vector2 RandomNoiseDelay = new(10f, 15f);
        [SerializeField] private Vector2 RandomNoiseDuration = new(.3f, 7f);

        #endregion

        #region Private Fields

        private MazeGenerator _generator;
        private List<IItemTrigger> _items = new();

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
            StopAllCoroutines();
        }

        #endregion

        #region Fonctions publiques

        /// <summary>
        /// Affiche l'effet de brouillage sans arr�t
        /// </summary>
        public void ShowNoiseNonStop()
        {
            _mapNoise.StopAllCoroutines();
            _mapNoise.ShowNoise(1f, .1f, Mathf.Infinity);
        }

        /// <summary>
        /// Affiche l'effet de brouillage par intermittence
        /// </summary>
        public void ShowNoiseAtRandom()
        {
            _mapNoise.StopAllCoroutines();
            StartCoroutine(DisplayNoiseCo());
        }

        #endregion

        #region Private Methods

        #region Events

        /// <summary>
        /// Quand le d�dale est g�n�r�, on lie les icones des objets aux ItemTriggers actifs
        /// et la carte au d�dale g�n�r� dans le MazeGenerator
        /// </summary>
        private void OnMazeDone(object sender, Procedural.MazeGeneration.GenerationProgressReport e)
        {
            if (!Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                return;
            }

            // Masque les croix des objets actifs par d�faut
            for (int i = 0; i < GameSession.Settings.ActiveItems.Length; i++)
            {
                if (GameSession.Settings.ActiveItems[i] == 2)
                {
                    ShowEnabledItem(i);
                }
            }

            // Pour les objets restants qui doivent �tre ramass�s, on leur assigne
            EntityTrigger[] entities = FindObjectsOfType<EntityTrigger>();
            foreach (var entity in entities)
            {
                if (entity is IItemTrigger item)
                {
                    if (item.Item.EntityType != EntityType.Exit)
                    {
                        _items.Add(item);
                        item.OnItemReachedEvent += OnItemReached;
                    }
                }
            }

            // Affiche le temps � l'�cran
            StartCoroutine(DisplayTimeCo());

            //Si la carte est active par d�faut
            if (GameSession.Settings.ActiveItems[1] == 2)
            {
                ShowNoiseAtRandom();
            }
            else
            {
                // Affiche l'effet de brouillage sur la carte
                // tant que la Map n'est pas ramass�e
                ShowNoiseNonStop();
            }
        }

        /// <summary>
        /// Quand un objet est ramass�
        /// </summary>
        /// <param name="sender">L'objet ramass�</param>
        /// <param name="e">Des infos sur l'objet ramass�</param>
        private void OnItemReached(object sender, ItemEventArgs e)
        {
            ShowEnabledItem(e.ID);
            _items[e.ID].OnItemReachedEvent -= OnItemReached;

            if(e.ID == 1)
            {
                ShowNoiseAtRandom();
            }
        }

        #endregion

        #region UI

        /// <summary>
        /// Affiche l'objet comme actif
        /// </summary>
        /// <param name="i">L'ID de l'objet dans la liste</param>
        /// 
        private void ShowEnabledItem(int i)
        {
            _itemCrosses[i].SetActive(false);
        }

        /// <summary>
        /// Affiche le temps � l'�cran
        /// </summary>
        private IEnumerator DisplayTimeCo()
        {
            while (true)
            {
                var span = System.TimeSpan.FromSeconds(GameSession.ElapsedTime);
                _timeText.text = $"{span.Minutes:00}:{span.Seconds:00}";
                yield return null;
            }
        }

        /// <summary>
        /// Affiche l'effet de brouillage sur la carte
        /// </summary>
        private IEnumerator DisplayNoiseCo()
        {
            // D�lai et dur�e de brouillage al�atoire entre chaque g�n�ration
            float randDelaySeconds = Random.Range(RandomNoiseDelay.x, RandomNoiseDelay.y);
            var wait = new WaitForSeconds(randDelaySeconds);

            while (true)
            {
                yield return wait;

                float randDurationSeconds = Random.Range(RandomNoiseDuration.x, RandomNoiseDuration.y);
                _mapNoise.ShowNoise(1f, .1f, randDurationSeconds);
            }
        }

        #endregion

        #endregion
    }
}