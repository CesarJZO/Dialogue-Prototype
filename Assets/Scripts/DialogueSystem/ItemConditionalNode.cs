using CesarJZO.InventorySystem;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Item Conditional Node", menuName = "Dialogue/Item Conditional Node", order = 3)]
    public class ItemConditionalNode : DialogueNode
    {
        [SerializeField] private Item hasItem;
        [SerializeField] private DialogueNode trueChild;
        [SerializeField] private DialogueNode falseChild;

        private Item _comparableItem;

        public Item Item => hasItem;

        public override DialogueNode Child => Evaluate() ? trueChild : falseChild;
        public override bool TryRemoveChild(DialogueNode node)
        {
            if (trueChild == node)
            {
                trueChild = null;
                return true;
            }
            if (falseChild == node)
            {
                falseChild = null;
                return true;
            }
            return false;
        }

        public void SetChild(DialogueNode node, bool which)
        {
            if (which)
                trueChild = node;
            else
                falseChild = node;
        }

        public DialogueNode GetChild(bool which)
        {
            return which ? trueChild : falseChild;
        }

        public void UnlinkChild(bool which)
        {
            if (which)
                trueChild = null;
            else
                falseChild = null;
        }

        public void SetItem(Item item)
        {
            _comparableItem = item;
        }

        public bool Evaluate()
        {
            return hasItem == _comparableItem;
        }
    }
}
