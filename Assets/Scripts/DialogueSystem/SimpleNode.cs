using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class SimpleNode : DialogueNode
    {
        [SerializeField] private DialogueNode child;

        public override DialogueNode Child => child;
        public override bool TryRemoveChild(DialogueNode node)
        {
            if (child != node) return false;

            child = null;
            return true;
        }

        public DialogueNode GetChild()
        {
            return child;
        }

        public void SetChild(DialogueNode node)
        {
            child = node;
        }

        public void UnlinkChild()
        {
            child = null;
        }
    }
}
