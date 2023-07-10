using CesarJZO.InventorySystem;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ItemConditionalNode : DialogueNode
    {
        public const string HasItemProperty = nameof(hasItem);

        [SerializeField] private Item hasItem;

        [SerializeField, HideInInspector] private DialogueNode trueChild;
        [SerializeField, HideInInspector] private DialogueNode falseChild;

        private Item _comparableItem;

        public Item Item
        {
            get => hasItem;
#if UNITY_EDITOR
            set
            {
                var serializedNode = new SerializedObject(this);
                SerializedProperty itemProperty = serializedNode.FindProperty("hasItem");
                itemProperty.objectReferenceValue = value;
                serializedNode.ApplyModifiedProperties();
            }
#endif
        }

        public override DialogueNode Child => Evaluate() ? trueChild : falseChild;

        public DialogueNode GetChild(bool which)
        {
            return which ? trueChild : falseChild;
        }

        public void SetItemToCompare(Item item)
        {
            _comparableItem = item;
        }

        public bool Evaluate()
        {
            return hasItem == _comparableItem;
        }

#if UNITY_EDITOR
        public void SetChild(DialogueNode node, bool which)
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty(which ? "trueChild" : "falseChild");
            childProperty.objectReferenceValue = node;
            serializedNode.ApplyModifiedProperties();
        }

        public void UnlinkChild(bool which)
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty(which ? "trueChild" : "falseChild");
            childProperty.objectReferenceValue = null;
            serializedNode.ApplyModifiedProperties();
        }

        public override bool TryRemoveChild(DialogueNode node)
        {
            var serializedNode = new SerializedObject(this);

            if (trueChild == node)
            {
                SerializedProperty property = serializedNode.FindProperty("trueChild");
                property.objectReferenceValue = null;
                serializedNode.ApplyModifiedProperties();
                return true;
            }

            if (falseChild == node)
            {
                SerializedProperty property = serializedNode.FindProperty("falseChild");
                property.objectReferenceValue = null;
                serializedNode.ApplyModifiedProperties();
                return true;
            }

            return false;
        }
#endif
    }
}
