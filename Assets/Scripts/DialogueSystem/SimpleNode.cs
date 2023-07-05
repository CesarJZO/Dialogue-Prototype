using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Simple Node", menuName = "Dialogue/Simple Node", order = 1)]
    public class SimpleNode : DialogueNode
    {
        [SerializeField] private DialogueNode child;

        public override DialogueNode Child => child;

        public bool HasChild => child;

        public void SetChild(DialogueNode node)
        {
            child = node;
        }

        public void RemoveChild()
        {
            child = null;
        }
    }
}
