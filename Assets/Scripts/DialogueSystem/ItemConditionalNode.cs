using CesarJZO.InventorySystem;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ItemConditionalNode : DialogueNode
    {
        [SerializeField] private Item hasItem;

        private DialogueNode _trueChild;
        private DialogueNode _falseChild;

        private Item _comparableItem;

        public Item Item => hasItem;

        public override DialogueNode Child => Evaluate() ? _trueChild : _falseChild;

        public override bool TryRemoveChild(DialogueNode node)
        {
            if (_trueChild == node)
            {
                _trueChild = null;
                return true;
            }

            if (_falseChild == node)
            {
                _falseChild = null;
                return true;
            }

            return false;
        }

        public void SetChild(DialogueNode node, bool which)
        {
            if (which)
                _trueChild = node;
            else
                _falseChild = node;
        }

        public DialogueNode GetChild(bool which)
        {
            return which ? _trueChild : _falseChild;
        }

        public void UnlinkChild(bool which)
        {
            if (which)
                _trueChild = null;
            else
                _falseChild = null;
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
