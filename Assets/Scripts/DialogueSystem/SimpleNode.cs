using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class SimpleNode : DialogueNode
    {
        [SerializeField, HideInInspector] private DialogueNode child;

        public override DialogueNode Child => child;

        public DialogueNode GetChild()
        {
            return child;
        }

#if UNITY_EDITOR
        public void SetChild(DialogueNode node)
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty("child");
            childProperty.objectReferenceValue = node;
            serializedNode.ApplyModifiedProperties();
        }

        public void UnlinkChild()
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty("child");
            childProperty.objectReferenceValue = null;
            serializedNode.ApplyModifiedProperties();
        }

        public override bool TryRemoveChild(DialogueNode node)
        {
            if (child != node) return false;

            UnlinkChild();

            return true;
        }
#endif
    }
}
