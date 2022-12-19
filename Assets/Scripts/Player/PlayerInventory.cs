using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Inventory", menuName = "Player/Inventory")]
    public class PlayerInventory : ScriptableObject
    {
        [SerializeField] private List<GameObject> items;
        
        public void AddItem(GameObject item) => items.Add(item);
    }
}
