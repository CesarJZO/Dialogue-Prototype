using CesarJZO.InventorySystem;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Item Conditional Node", menuName = "Dialogue/Item Conditional Node", order = 3)]
    public class ItemConditionalNode : DialogueNode
    {
        [SerializeField] private DialogueNode trueChild;
        [SerializeField] private DialogueNode falseChild;
        [SerializeField] private Item hasItem;

        private Item _comparableItem;

        public Item Item => hasItem;

        public override DialogueNode Child => Evaluate() ? trueChild : falseChild;

        public void SetTrueChild(DialogueNode node)
        {
            trueChild = node;
        }

        public void SetFalseChild(DialogueNode node)
        {
            falseChild = node;
        }

        public void SetItem(Item item)
        {
            _comparableItem = item;
        }

        public DialogueNode GetChild(bool value)
        {
            return value ? trueChild : falseChild;
        }

        public bool Evaluate()
        {
            return hasItem == _comparableItem;
        }
    }
}
