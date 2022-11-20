using System;
using Project.Models.Game;
using Project.Models.Game.Enums;
using UnityEngine;

namespace Project.ViewModels.Entities.Items
{

    /// <summary>
    /// Le trigger de la clé déverrouillant le portail.
    /// Présent uniquement en mode Facile ou Perso (si actif).
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Key : EntityTrigger, IItemTrigger
    {
        #region Events

        /// <summary>
        /// Quand le portail est atteint
        /// </summary>
        public EventHandler<KeyEventArgs> OnKeyReachedEvent = delegate { };

        #endregion

        #region Public Fields

        [field: SerializeField]
        public Item Item { get; set; }

        #endregion

        #region Mono

        /// <summary>
        /// En mode Facile ou en mode Perso avec la clé désactivée,
        /// on détruit la clé pour ne pas la ramasser,
        /// puisque le portail est actif par défauts
        /// </summary>
        private void Start()
        {
            if (GameSession.DifficultyLevel == Difficulty.Easy ||
               GameSession.DifficultyLevel == Difficulty.Custom &&
               GameSession.Settings.ActiveItems[0] == 0)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void OnTrigger(IEntity entity)
        {
            // Quand atteint, déverouille le portail
            // et détruit cet objet
            if (entity.EntityType == EntityType.Player)
            {
                FindObjectOfType<Portal>().IsUnlocked = true;
                Destroy(gameObject);

                //AF: Afficher un message en jeu pour indiquer
                //au joueur qu'il doit ramasser la clé
                OnKeyReachedEvent(this, new KeyEventArgs());
            }
        }

        #endregion
    }
}