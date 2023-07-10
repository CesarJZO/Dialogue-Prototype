using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class SimpleNode : DialogueNode
    {
        public const string ChildProperty = nameof(child);

        [SerializeField, HideInInspector] private DialogueNode child;

        public override DialogueNode Child => child;

        public DialogueNode GetChild()
        {
            return child;
        }

#if UNITY_EDITOR
        public override bool TryRemoveChild(DialogueNode node)
        {
            if (child != node) return false;

            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty(ChildProperty);
            childProperty.objectReferenceValue = null;
            serializedNode.ApplyModifiedProperties();

            return true;
        }
#endif
    }
}
