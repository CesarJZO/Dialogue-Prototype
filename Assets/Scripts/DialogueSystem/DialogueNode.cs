using System;
using System.Collections.Generic;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string conversant;
        [SerializeField, TextArea] private string text;
        [SerializeField] private List<DialogueNode> children;

        [HideInInspector] public Rect rect = new(0f, 0f, 200f, 120f);

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
