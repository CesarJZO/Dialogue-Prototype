using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public abstract class ConditionalNode : DialogueNode
    {
        [SerializeField] protected DialogueNode trueChild;
        [SerializeField] protected DialogueNode falseChild;

        public void SetTrueChild(DialogueNode node)
        {
            trueChild = node;
        }

        public void SetFalseChild(DialogueNode node)
        {
            falseChild = node;
        }
    }
}
