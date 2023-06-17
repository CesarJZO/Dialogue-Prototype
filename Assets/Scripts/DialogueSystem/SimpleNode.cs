using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class SimpleNode : DialogueNode
    {
        [SerializeField] private DialogueNode child;

        public override DialogueNode Child => child;

        private void Awake()
        {
            Initialize();
        }

        public void SetChild(DialogueNode node)
        {
            child = node;
        }
    }
}
