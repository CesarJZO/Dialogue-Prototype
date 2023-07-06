using CesarJZO.InventorySystem;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ItemConditionalNode : DialogueNode
    {
        [SerializeField] private Item hasItem;

        [SerializeField, HideInInspector] private DialogueNode trueChild;
        [SerializeField, HideInInspector] private DialogueNode falseChild;

        private Item _comparableItem;

        public Item Item => hasItem;

        public override DialogueNode Child => Evaluate() ? trueChild : falseChild;

        public DialogueNode GetChild(bool which)
        {
            return which ? trueChild : falseChild;
        }

        public void SetItemToCompare(Item item)
        {
            _comparableItem = item;
        }

        public bool Evaluate()
        {
            return hasItem == _comparableItem;
        }

#if UNITY_EDITOR
        public void SetChild(DialogueNode node, bool which)
        {
            if (which)
                trueChild = node;
            else
                falseChild = node;
            EditorUtility.SetDirty(this);
        }

        public void UnlinkChild(bool which)
        {
            if (which)
                trueChild = null;
            else
                falseChild = null;
            EditorUtility.SetDirty(this);
        }

        public override bool TryRemoveChild(DialogueNode node)
        {
            if (trueChild == node)
            {
                trueChild = null;
                EditorUtility.SetDirty(this);
                return true;
            }

            if (falseChild == node)
            {
                falseChild = null;
                EditorUtility.SetDirty(this);
                return true;
            }

            return false;
        }
#endif
    }
}
