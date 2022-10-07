using Project.Models.Game.Enums;
using UnityEngine;

namespace Project.Models.Game
{
    public interface IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Sprite Icon { get; set; }
        public GameObject Prefab { get; set; }
        public EntityType EntityType { get; set; }
    }
}