using UnityEngine;

namespace CesarJZO.InventorySystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
    public class Item : ScriptableObject
    {
        [SerializeField] private string displayName;

        public string DisplayName => displayName;
    }
}
