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
    }

    public interface IItemTrigger<TEventArgs> : IItemTrigger where TEventArgs : ItemEventArgs
    {
        /// <summary>
        /// Quand l'objet est activé
        /// </summary>
        public EventHandler<TEventArgs> OnItemReachedEvent { get; set; }
    }
}