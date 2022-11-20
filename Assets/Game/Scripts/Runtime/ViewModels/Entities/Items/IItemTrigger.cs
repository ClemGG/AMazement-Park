using System;
using Project.Models.Game;

namespace Project.ViewModels.Entities.Items
{
    public interface IItemTrigger<TEventArgs> where TEventArgs : ItemEventArgs
    {
        /// <summary>
        /// Quand l'objet est activ�
        /// </summary>
        public EventHandler<TEventArgs> OnItemReachedEvent { get; set; }

        Item Item { get; set; }

        /// <summary>
        /// Quand le script de l'entit� appelle son OnTriggerEnter
        /// </summary>
        void OnTrigger(IEntity entity);
    }
}