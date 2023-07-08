using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ResponseNode : DialogueNode
    {
        [SerializeField] [Min(0f)] private float timeLimit;

        [SerializeField] private List<Response> responses;

        public float TimeLimit
        {
            get => timeLimit;
#if UNITY_EDITOR
            set
            {
                var serializedNode = new SerializedObject(this);
                SerializedProperty timeLimitProperty = serializedNode.FindProperty("timeLimit");
                timeLimitProperty.floatValue = value;
                serializedNode.ApplyModifiedProperties();
            }
#endif
        }

        public Response CurrentResponse { set; get; }

        public override DialogueNode Child => CurrentResponse?.Child;

        public IEnumerable<Response> Responses => responses;

        public int ChildrenCount => responses.Count;

        public string GetText(int index)
        {
            return responses[index].Text;
        }

        public DialogueNode GetChild(int index)
        {
            return responses[index].Child;
        }

#if UNITY_EDITOR
        private void Awake()
        {
            responses ??= new List<Response>();
        }

        public void UnlinkChild(int index)
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty($"responses.Array.data[{index}].child");
            childProperty.objectReferenceValue = null;
            serializedNode.ApplyModifiedProperties();
        }

        public void AddResponse()
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty responsesProperty = serializedNode.FindProperty("responses");
            responsesProperty.InsertArrayElementAtIndex(responses.Count);
            serializedNode.ApplyModifiedProperties();
        }

        public void RemoveResponse()
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty responsesProperty = serializedNode.FindProperty("responses");
            responsesProperty.DeleteArrayElementAtIndex(responses.Count - 1);
            serializedNode.ApplyModifiedProperties();
        }

        public void SetText(string text, int index)
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty textProperty = serializedNode.FindProperty($"responses.Array.data[{index}].text");
            textProperty.stringValue = text;
            serializedNode.ApplyModifiedProperties();
        }

        public void SetChild(DialogueNode node, int index)
        {
            var serializedNode = new SerializedObject(this);
            SerializedProperty childProperty = serializedNode.FindProperty($"responses.Array.data[{index}].child");
            childProperty.objectReferenceValue = node;
            serializedNode.ApplyModifiedProperties();
        }

        public override bool TryRemoveChild(DialogueNode node)
        {
            int index = responses.FindIndex(response => response.Child == node);

            if (index == -1) return false;

            UnlinkChild(index);

            return true;
        }
#endif
    }

    [Serializable]
    public class Response
    {
        [SerializeField] private string text;
        [SerializeField] private string trigger;
        [SerializeField, HideInInspector] private DialogueNode child;

        public string Text => text;

        public string Trigger => trigger;

        public DialogueNode Child => child;
    }
}
