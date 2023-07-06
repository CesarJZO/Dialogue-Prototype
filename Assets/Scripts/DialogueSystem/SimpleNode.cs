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

        public override bool TryRemoveChild(DialogueNode node)
        {
            if (child != node) return false;
            child = null;
            EditorUtility.SetDirty(this);
            return true;
        }
#endif
    }
}
