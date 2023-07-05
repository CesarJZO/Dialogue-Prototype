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
        public DialogueNode RootNode => nodes[0];

        public IEnumerable<DialogueNode> Nodes => nodes;

        private void Awake()
        {
            nodes = new List<DialogueNode>();
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

        public void AddNode(DialogueNode parent, NodeType type = NodeType.SimpleNode)
        {
            DialogueNode simpleNode = type switch
            {
                NodeType.ResponseNode => CreateInstance<ResponseNode>(),
                NodeType.ConditionalNode => CreateInstance<ItemConditionalNode>(),
                _ => CreateInstance<SimpleNode>()
            };
            simpleNode.name = Guid.NewGuid().ToString();
            nodes.Add(simpleNode);

            SaveInstance(simpleNode);
        }

        private void SetSimpleNodeChild(SimpleNode parent, DialogueNode child)
        {
            parent.SetChild(child);
        }

        private void SetConditionalNodeChild(ItemConditionalNode parent, DialogueNode child, bool which)
        {
            if (which)
                parent.SetTrueChild(child);
            else
                parent.SetFalseChild(child);
        }

        private void SetResponseNodeChild(ResponseNode parent, DialogueNode child, int index)
        {
            parent.SetChild(child, index);
        }

        public void RemoveNode(DialogueNode node)
        {
            nodes.Remove(node);
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
    }
}
