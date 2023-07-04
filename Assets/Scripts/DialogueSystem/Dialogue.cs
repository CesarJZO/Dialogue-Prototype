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

        public void AddSimpleNode(DialogueNode parent)
        {
            var simpleNode = CreateInstance<SimpleNode>();

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

        [MenuItem("Tools/Simple Node", priority = 0)]
        public static void AddSimpleNode()
        {
            var dialogue = Selection.activeObject as Dialogue;

            if (!dialogue) return;

            Debug.Log($"Adding simple node to {dialogue.name}");

            dialogue.AddSimpleNode(parent: null);
        }
    }
}
