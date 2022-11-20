using System;
using Project.Models.Game;
using Project.Models.Game.Enums;
using UnityEngine;

namespace Project.ViewModels.Entities.Items
{

    /// <summary>
    /// Le trigger débloquant la position des ennemis et objets sur la carte
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Tracker : EntityTrigger, IItemTrigger<TrackerEventArgs>
    {
        #region Events

        /// <summary>
        /// Quand le portail est atteint
        /// </summary>
        public EventHandler<TrackerEventArgs> OnItemReachedEvent { get; set; } = delegate { };

        #endregion

        #region Public Fields

        [field: SerializeField]
        public Item Item { get; set; }

        #endregion

        #region Mono

        /// <summary>
        /// En mode Facile ou en mode Perso avec la carte désactivée,
        /// on ne laisse que la carte par défaut, pas la cate entière
        /// </summary>
        private void Start()
        {
            if (GameSession.DifficultyLevel == Difficulty.Easy ||
               GameSession.DifficultyLevel == Difficulty.Custom &&
               GameSession.Settings.ActiveItems[0] != 1)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void OnTrigger(IEntity entity)
        {
            // Quand atteint, débloque la position des ennemis et objets sur la carte
            if (entity.EntityType == EntityType.Player)
            {
                Destroy(gameObject);

                OnItemReachedEvent(this, new TrackerEventArgs());
            }
        }

        #endregion
    }
}