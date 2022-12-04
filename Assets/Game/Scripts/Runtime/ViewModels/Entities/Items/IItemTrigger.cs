using System;
using Project.Models.Game;
using Project.ViewModels.Entities.EventArgs;

namespace Project.ViewModels.Entities.Items
{
    public interface IItemTrigger
    {
        Item Item { get; set; }

        /// <summary>
        /// Quand le script de l'entité appelle son OnTriggerEnter
        /// </summary>
        void OnTrigger(IEntity entity);

        /// <summary>
        /// Quand l'objet est activé
        /// </summary>
        EventHandler<ItemEventArgs> OnItemReachedEvent { get; set; }
    }
}