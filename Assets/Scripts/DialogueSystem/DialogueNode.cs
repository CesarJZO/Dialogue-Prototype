using System;
using System.Collections.Generic;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string conversant;
        [SerializeField, TextArea] private string text;
        [SerializeField] private DialogueNode parent;
        [SerializeField] private List<DialogueNode> children;
        [SerializeField] private bool childrenAreResponses;
        [HideInInspector] public Rect rect = new(0f, 0f, 256f, 120f);

        public IEnumerable<DialogueNode> Children => children.AsReadOnly();

        public string Conversant
        {
            get => conversant;
#if UNITY_EDITOR
            set => conversant = value;
#endif
        }

        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set => text = value;
#endif
        }

        public DialogueNode Parent
        {
            get => parent;
#if UNITY_EDITOR
            set => parent = value;
#endif
        }

        public bool ChildrenAreResponses
        {
            get => childrenAreResponses;
#if UNITY_EDITOR
            set => childrenAreResponses = value;
#endif
        }

#if UNITY_EDITOR

        private void Awake()
        {
            name = Guid.NewGuid().ToString();
            children = new List<DialogueNode>();
        }

        public void AddChild(DialogueNode child) => children.Add(child);
#endif
    }
}
