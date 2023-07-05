using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Simple Node", menuName = "Dialogue/Simple Node", order = 1)]
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
