using UnityEngine;

namespace CesarJZO.InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Item", order = 1)]
    public class Item : ScriptableObject
    {
        [SerializeField] private string displayName;

        public string DisplayName => displayName;
    }
}
