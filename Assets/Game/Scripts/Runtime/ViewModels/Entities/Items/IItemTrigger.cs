using Project.Models.Game;

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
}