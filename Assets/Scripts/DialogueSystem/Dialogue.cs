using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes;
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
            childNode.rect.position = parent.rect.position + new Vector2(parent.rect.width + 50f, 0f);

            nodes.Add(childNode);
            SaveInstance(childNode);

            return childNode;
        }

        public void CreateNodeAtPoint(NodeType type, Vector2 position)
        {
            DialogueNode node = type switch
            {
                NodeType.ConditionalNode => CreateInstance<ItemConditionalNode>(),
                NodeType.ResponseNode => CreateInstance<ResponseNode>(),
                _ => CreateInstance<SimpleNode>()
            };
            node.name = GetGuidFormatted(type);
            node.rect.position = position;

            nodes.Add(node);
            SaveInstance(node);
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
            {
                if (n.IsChild(node))
                {
                    n.RemoveChild(node);
                }
            }
            RemoveInstanceFromAssetDatabase(node);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void SetNodeAsRoot(DialogueNode node)
        {
            if (!nodes.Contains(node))
            {
                Debug.LogWarning("Node is not in the dialogue.");
                return;
            }

            nodes.Remove(node);
            nodes.Insert(0, node);
        }

        private static string GetGuidFormatted(NodeType type)
        {
            ReadOnlySpan<char> guidSpan = Guid.NewGuid().ToString();
            ReadOnlySpan<char> typeSpan = type.ToString();
            return $"{typeSpan[0]}-{guidSpan[..8].ToString()}";
        }

        public bool IsRoot(DialogueNode node) => node == RootNode;
    }
}
