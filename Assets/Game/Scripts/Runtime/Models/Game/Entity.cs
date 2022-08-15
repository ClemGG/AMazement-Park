using UnityEngine;

namespace Project.Models.Game
{
    public interface IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Sprite Icon { get; set; }
    }
}