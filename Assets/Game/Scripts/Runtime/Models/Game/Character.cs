using UnityEngine;

namespace Project.Models.Game
{
    [CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Entities/Character", order = 1)]
    public class Character : ScriptableObject, IEntity
    {
        [field: SerializeField] public int ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
    }
}