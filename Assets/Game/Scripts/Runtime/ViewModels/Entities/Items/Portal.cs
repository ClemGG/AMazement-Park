using System;
using Project.Models.Game;
using Project.Models.Game.Enums;
using Project.ViewModels.Entities.EventArgs;
using UnityEngine;

namespace Project.ViewModels.Entities.Items
{
    /// <summary>
    /// Le trigger du portail de sortie.
    /// Quand atteint, termine la Session, incrément le nombre de victoires
    /// et lance la scène de transition vers le niveau suivant
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Portal : EntityTrigger, IItemTrigger<PortalEventArgs>
    {
        #region Events

        /// <summary>
        /// Quand le portail est atteint
        /// </summary>
        public EventHandler<PortalEventArgs> OnItemReachedEvent { get; set; } = delegate { };

        #endregion

        #region Public Fields

        [field: SerializeField]
        public Item Item { get; set; }

        public bool IsUnlocked { get; set; } = false;

        #endregion

        #region Mono

        private void Start()
        {
            //Par défaut le portail est désactivé, sauf en mode Facile
            //ou en Mode Custom avec la clé désactivée
            IsUnlocked = GameSession.DifficultyLevel == Models.Game.Enums.Difficulty.Easy ||
                         GameSession.DifficultyLevel == Difficulty.Custom && GameSession.Settings.ActiveItems[0] == 1;
        }

        #endregion

        #region Public Methods

        public void OnTrigger(IEntity entity)
        {
            // Quand atteint, termine la Session, incrément le nombre de victoires
            // et lance la scène de transition vers le niveau suivant
            if (entity.EntityType == EntityType.Player)
            {
                //AF: Afficher un message en jeu pour indiquer
                //au joueur qu'il doit ramasser la clé
                OnItemReachedEvent(this, new PortalEventArgs(IsUnlocked));
            }
        }

        #endregion
    }
}