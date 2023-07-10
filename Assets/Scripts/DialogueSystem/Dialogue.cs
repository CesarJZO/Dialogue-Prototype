using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField, HideInInspector] private List<DialogueNode> nodes;

        public DialogueNode RootNode => nodes.Count == 0 ? null : nodes[0];

        public IEnumerable<DialogueNode> Nodes => nodes;

#if UNITY_EDITOR
        private void Awake()
        {
            nodes ??= new List<DialogueNode>();
        }

        private void SaveInstance(DialogueNode node)
        {
            if (AssetDatabase.Contains(node)) return;
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
        }

        private void RemoveInstanceFromAssetDatabase(DialogueNode node)
        {
            if (!AssetDatabase.Contains(node)) return;
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        private DialogueNode CreateNode(DialogueNode parent, DialogueNodeType childType)
        {
            DialogueNode childNode = childType switch
            {
                DialogueNodeType.ConditionalNode => CreateInstance<ItemConditionalNode>(),
                DialogueNodeType.ResponseNode => CreateInstance<ResponseNode>(),
                _ => CreateInstance<SimpleNode>()
            };
            childNode.name = GetGuidFormatted(childType);
            if (parent)
                childNode.rect.position = parent.rect.position + new Vector2(parent.rect.width + 64f, 0f);

            nodes.Add(childNode);
            SaveInstance(childNode);

            return childNode;
        }

        public void CreateNodeAtPoint(DialogueNodeType type, Vector2 position)
        {
            DialogueNode node = CreateNode(null, type);
            node.rect.position = position;
        }

        public void AddChildToSimpleNode(SimpleNode parent, DialogueNodeType childType)
        {
            DialogueNode childNode = CreateNode(parent, childType);
            var serializedObject = new SerializedObject(parent);
            SerializedProperty childProperty = serializedObject.FindProperty(SimpleNode.ChildProperty);
            childProperty.objectReferenceValue = childNode;
            serializedObject.ApplyModifiedProperties();
        }

        public void AddChildToConditionalNode(ItemConditionalNode parent, DialogueNodeType childType, bool condition)
        {
            DialogueNode childNode = CreateNode(parent, childType);
            parent.SetChild(childNode, condition);
        }

        public void AddChildToResponseNode(ResponseNode parent, DialogueNodeType childType, int index)
        {
            DialogueNode childNode = CreateNode(parent, childType);
            parent.SetChild(childNode, index);
        }

        public void SetNodeAsRoot(DialogueNode node)
        {
            if (!nodes.Contains(node))
                return;

            nodes.Remove(node);
            nodes.Insert(0, node);
        }
#endif

        private static string GetGuidFormatted(DialogueNodeType type)
        {
            ReadOnlySpan<char> guidSpan = Guid.NewGuid().ToString();
            ReadOnlySpan<char> typeSpan = type.ToString();
            return typeSpan[0].ToString() + '-' + guidSpan[..8].ToString();
        }

        public bool IsRoot(DialogueNode node)
        {
            return node == RootNode;
        }

        public int IndexOf(DialogueNode node)
        {
            return nodes.IndexOf(node);
        }
    }
}
