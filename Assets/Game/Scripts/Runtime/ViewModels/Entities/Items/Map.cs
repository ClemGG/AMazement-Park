using System;
using Project.Models.Game;
using Project.Models.Game.Enums;
using Project.ViewModels.Entities.EventArgs;
using UnityEngine;

namespace Project.ViewModels.Entities.Items
{

    /// <summary>
    /// Le trigger d�bloquant la carte compl�te
    /// (Par d�faut, seules les cases d�j� visit�es sont affich�es)
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Map : EntityTrigger, IItemTrigger<MapEventArgs>
    {
        #region Events

        /// <summary>
        /// Quand le portail est atteint
        /// </summary>
        public EventHandler<MapEventArgs> OnItemReachedEvent { get; set; } = delegate { };

        #endregion

        #region Public Fields

        [field: SerializeField]
        public Item Item { get; set; }

        #endregion

        #region Mono

        /// <summary>
        /// En mode Facile ou en mode Perso avec la carte d�sactiv�e,
        /// on ne laisse que la carte par d�faut, pas la cate enti�re
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
            // Quand atteint, d�bloque la carte enti�re du niveau
            if (entity.EntityType == EntityType.Player)
            {
                Destroy(gameObject);

                OnItemReachedEvent(this, new MapEventArgs());
            }
        }

        #endregion
    }
}