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
            child = node;
            EditorUtility.SetDirty(this);
        }

        public void UnlinkChild()
        {
            child = null;
            EditorUtility.SetDirty(this);
        }
#endif

        public override bool TryRemoveChild(DialogueNode node)
        {
            if (child != node) return false;
#if UNITY_EDITOR
            child = null;
            EditorUtility.SetDirty(this);
#endif
            return true;
        }
    }
}
