#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem.Editor
{
    public static class DialogueEditor
    {
        private static void SaveInstance(this Dialogue dialogue, DialogueNode node)
        {
            if (AssetDatabase.Contains(node)) return;
            AssetDatabase.AddObjectToAsset(node, dialogue);
            AssetDatabase.SaveAssets();
        }

        private static void RemoveInstance(this Dialogue dialogue, DialogueNode node)
        {
            if (!AssetDatabase.Contains(node)) return;
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        private static DialogueNode CreateNode(this Dialogue dialogue, DialogueNode parent, NodeType childType)
        {
            DialogueNode childNode = childType switch
            {
                NodeType.ConditionalNode => ScriptableObject.CreateInstance<ItemConditionalNode>(),
                NodeType.ResponseNode => ScriptableObject.CreateInstance<ResponseNode>(),
                _ => ScriptableObject.CreateInstance<SimpleNode>()
            };
            childNode.name = GetGuidFormatted(childType);
            if (parent)
                childNode.rect.position = parent.rect.position + new Vector2(parent.rect.width + 50f, 0f);

            var serializedObject = new SerializedObject(dialogue);
            SerializedProperty nodesProperty = serializedObject.FindProperty(Dialogue.NodesProperty);
            nodesProperty.InsertArrayElementAtIndex(0);
            nodesProperty.GetArrayElementAtIndex(0).objectReferenceValue = childNode;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            dialogue.SaveInstance(childNode);

            return childNode;
        }

        public static void CreateNodeAtPoint(this Dialogue dialogue, NodeType type, Vector2 position)
        {
            DialogueNode node = dialogue.CreateNode(null, type);
            node.rect.position = position;
        }

        public static void AddChildToSimpleNode(this Dialogue dialogue, SimpleNode parent, NodeType childType)
        {
            DialogueNode childNode = dialogue.CreateNode(parent, childType);
            parent.SetChild(childNode);
        }

        public static void AddChildToConditionalNode(this Dialogue dialogue, ItemConditionalNode parent, NodeType childType, bool condition)
        {
            DialogueNode childNode = dialogue.CreateNode(parent, childType);
            parent.SetChild(childNode, condition);
        }

        public static void AddChildToResponseNode(this Dialogue dialogue, ResponseNode parent, NodeType childType, int index)
        {
            DialogueNode childNode = dialogue.CreateNode(parent, childType);
            parent.SetChild(childNode, index);
        }

        public static void RemoveNode(this Dialogue dialogue, DialogueNode node)
        {
            var serializedObject = new SerializedObject(dialogue);
            SerializedProperty nodesProperty = serializedObject.FindProperty(Dialogue.NodesProperty);
            for (int i = 0; i < nodesProperty.arraySize; i++)
            {
                if (nodesProperty.GetArrayElementAtIndex(i).objectReferenceValue != node) continue;

                nodesProperty.DeleteArrayElementAtIndex(i);
                break;
            }
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            foreach (DialogueNode n in dialogue.Nodes)
                n.TryRemoveChild(node);

            dialogue.RemoveInstance(node);
        }

        public static void SetNodeAsRoot(this Dialogue dialogue, DialogueNode node)
        {
            if (!dialogue.Nodes.Contains(node))
                return;

            var serializedObject = new SerializedObject(dialogue);
            SerializedProperty nodesProperty = serializedObject.FindProperty(Dialogue.NodesProperty);
            for (int i = 0; i < nodesProperty.arraySize; i++)
            {
                if (nodesProperty.GetArrayElementAtIndex(i).objectReferenceValue != node) continue;

                nodesProperty.DeleteArrayElementAtIndex(i);
                break;
            }

            nodesProperty.InsertArrayElementAtIndex(0);
            nodesProperty.GetArrayElementAtIndex(0).objectReferenceValue = node;
        }

        private static string GetGuidFormatted(NodeType type)
        {
            return $"{type.ToString()[0]}-{Guid.NewGuid()}";
        }
    }
}
#endif
