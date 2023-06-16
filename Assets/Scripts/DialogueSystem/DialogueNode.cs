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

        private void Awake()
        {
            name = Guid.NewGuid().ToString();

            children = new List<DialogueNode>();
        }

        public void AddChild(DialogueNode child) => children.Add(child);
    }
}
