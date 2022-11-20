using Project.Models.Game;
using Project.ViewModels.Entities.Items;
using UnityEngine;

namespace Project.ViewModels.Entities.Characters
{

    /// <summary>
    /// G�re les interactions du joueur avec les monstres
    /// et les objets lors d'une Session
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Public Fields

        [field: SerializeField]
        public Character Character { get; set; }

        #endregion

        #region Mono

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EntityTrigger trigger))
            {
                switch (trigger)
                {
                    case IItemTrigger itemT:
                        itemT.OnTrigger(Character);
                        break;
                }
            }
        }

        #endregion
    }
}