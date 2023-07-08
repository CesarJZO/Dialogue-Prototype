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
#endif

        public bool IsRoot(DialogueNode node)
        {
            return node == RootNode;
        }

        public static string NodesProperty => nameof(nodes);
    }
}
