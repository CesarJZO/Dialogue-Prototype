using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Conditional Node", menuName = "Dialogue/Conditional Node", order = 3)]
    public class ConditionalNode : DialogueNode
    {
        [SerializeField] private bool condition;
        [SerializeField] private DialogueNode trueChild;
        [SerializeField] private DialogueNode falseChild;

        public override DialogueNode Child => condition ? trueChild : falseChild;

        private void Awake()
        {
            Initialize();
        }

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
