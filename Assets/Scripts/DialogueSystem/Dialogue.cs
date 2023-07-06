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

        private DialogueNode CreateNode(DialogueNode parent, NodeType childType)
        {
            DialogueNode childNode = childType switch
            {
                NodeType.ConditionalNode => CreateInstance<ItemConditionalNode>(),
                NodeType.ResponseNode => CreateInstance<ResponseNode>(),
                _ => CreateInstance<SimpleNode>()
            };
            childNode.name = GetGuidFormatted(childType);
            if (parent)
                childNode.rect.position = parent.rect.position + new Vector2(parent.rect.width + 50f, 0f);

            nodes.Add(childNode);
            SaveInstance(childNode);

            return childNode;
        }

        public void CreateNodeAtPoint(NodeType type, Vector2 position)
        {
            DialogueNode node = CreateNode(null, type);
            node.rect.position = position;
        }

        public void AddChildToSimpleNode(SimpleNode parent, NodeType childType)
        {
            DialogueNode childNode = CreateNode(parent, childType);
            parent.SetChild(childNode);
        }

        public void AddChildToConditionalNode(ItemConditionalNode parent, NodeType childType, bool condition)
        {
            DialogueNode childNode = CreateNode(parent, childType);
            parent.SetChild(childNode, condition);
        }

        public void AddChildToResponseNode(ResponseNode parent, NodeType childType, int index)
        {
            DialogueNode childNode = CreateNode(parent, childType);
            parent.SetChild(childNode, index);
        }

        public void RemoveNode(DialogueNode node)
        {
            nodes.Remove(node);
            foreach (DialogueNode n in nodes)
                n.TryRemoveChild(node);

            RemoveInstanceFromAssetDatabase(node);
        }

        public void SetNodeAsRoot(DialogueNode node)
        {
            if (!nodes.Contains(node))
                return;

            nodes.Remove(node);
            nodes.Insert(0, node);
        }

        private static string GetGuidFormatted(NodeType type)
        {
            ReadOnlySpan<char> guidSpan = Guid.NewGuid().ToString();
            ReadOnlySpan<char> typeSpan = type.ToString();
            return typeSpan[0].ToString() + '-' + guidSpan[..8].ToString();
        }

        public bool IsRoot(DialogueNode node)
        {
            return node == RootNode;
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            foreach (DialogueNode node in nodes)
                EditorUtility.SetDirty(node);
            AssetDatabase.SaveAssets();
        }
    }
}
