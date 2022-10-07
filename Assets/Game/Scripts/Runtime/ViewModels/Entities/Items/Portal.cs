using Project.Models.Game;
using Project.Models.Game.Enums;
using UnityEngine;

namespace Project.ViewModels.Entities.Items
{
    /// <summary>
    /// Le trigger du portail de sortie.
    /// Quand atteint, termine la Session, incr�ment le nombre de victoires
    /// et lance la sc�ne de transition vers le niveau suivant
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Portal : EntityTrigger, IItemTrigger
    {
        #region Public Fields

        [field: SerializeField]
        public Item Item { get; set; }
        public bool IsUnlocked { get; set; } = false;

        #endregion

        #region Mono

        private void Start()
        {
            //Par d�faut le portail est d�sactiv�, sauf en mode Facile
            IsUnlocked = GameSession.DifficultyLevel == Models.Game.Enums.Difficulty.Easy;
        }

        #endregion

        #region Public Methods

        public void OnTrigger(IEntity entity)
        {
            // Quand atteint, termine la Session, incr�ment le nombre de victoires
            // et lance la sc�ne de transition vers le niveau suivant
            if(entity.EntityType == EntityType.Player)
            {

            }
        }

        #endregion
    }
}