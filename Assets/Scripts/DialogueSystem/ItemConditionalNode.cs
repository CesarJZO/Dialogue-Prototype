using CesarJZO.InventorySystem;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Item Conditional Node", menuName = "Dialogue/Item Conditional Node", order = 3)]
    public class ItemConditionalNode : ConditionalNode
    {
        [SerializeField] private Item hasItem;

        private Item _comparableItem;

        public Item Item => hasItem;

        public override DialogueNode Child => Evaluate() ? trueChild : falseChild;

        public void SetItem(Item item)
        {
            _comparableItem = item;
        }

        public override bool Evaluate()
        {
            return hasItem == _comparableItem;
        }
    }
}
