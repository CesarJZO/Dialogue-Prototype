using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [RequireComponent(typeof(DialogueNpc))]
    public class DialogueCondition : MonoBehaviour
    {
        private const string DialogueConditionTag = "DialogueCondition";

        [SerializeField] private ResponseAction action;
        [SerializeField] private Condition condition;

        [SerializeField] private string flag;

        [SerializeField] private Dialogue dialogueToReplace;

        public ResponseAction Action => action;
        public Dialogue DialogueToReplace => dialogueToReplace;

        public bool Evaluate(DialogueNode parent)
        {
            return condition switch
            {
                Condition.HadItem when parent is ItemConditionalNode itemConditionalNode =>
                    itemConditionalNode.Evaluate(),
                Condition.Flag => parent.Flag == flag,
                _ => false
            };
        }

        public bool EvaluateFlag(string flag)
        {
            return this.flag == flag;
        }

        private enum Condition
        {
            HadItem,
            Flag
        }

        public enum ResponseAction
        {
            Dequeue,
            Replace
        }
    }
}
