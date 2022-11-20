using System;
using Project.Models.Game;

namespace Project.ViewModels.Entities.Items
{
    public interface IItemTrigger<TEventArgs> where TEventArgs : ItemEventArgs
    {
        /// <summary>
        /// Quand l'objet est activé
        /// </summary>
        public EventHandler<TEventArgs> OnItemReachedEvent { get; set; }

        Item Item { get; set; }

        /// <summary>
        /// Quand le script de l'entité appelle son OnTriggerEnter
        /// </summary>
        void OnTrigger(IEntity entity);
    }
}