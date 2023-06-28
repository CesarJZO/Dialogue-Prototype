using CesarJZO.Core;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Flag Conditional Node", menuName = "Dialogue/Flag Conditional Node", order = 3)]
    public class FlagConditionalNode : ConditionalNode
    {
        [SerializeField] private GameFlag flag;
        [SerializeField] private GameFlags flags;

        public override DialogueNode Child => Evaluate() ? trueChild : falseChild;

        public override bool Evaluate()
        {
            return flags.CheckFlag(flag);
        }
    }
}
