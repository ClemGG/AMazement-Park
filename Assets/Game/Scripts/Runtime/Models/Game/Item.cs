using UnityEngine;

namespace Project.Models.Game
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Entities/Item", order = 2)]
    public class Item : ScriptableObject, IEntity
    {
        [field: SerializeField] public int ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
    }
}