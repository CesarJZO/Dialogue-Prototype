using System;
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

        public void SetItem(Item item)
        {
            _comparableItem = item;
        }

        public override DialogueNode Child => hasItem == _comparableItem ? trueChild : falseChild;
    }
}
