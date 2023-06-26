using CesarJZO.InventorySystem;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Item Conditional Node", menuName = "Dialogue/Item Conditional Node", order = 3)]
    public class ItemConditionalNode : ConditionalNode
    {
        [SerializeField] private Item hasItem;
        [SerializeField] private Inventory inventory;

        public override DialogueNode Child => inventory.HasItem(hasItem) ? trueChild : falseChild;
    }
}
