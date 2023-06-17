using System;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        [SerializeField] private Speaker speaker;
        [SerializeField, TextArea] private string text;
        [HideInInspector] public Rect rect = new(0f, 0f, 256f, 120f);

        public abstract DialogueNode Child { get; }

        public Speaker Speaker => speaker;

        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set => text = value;
#endif
        }

        protected void Initialize()
        {
            name = Guid.NewGuid().ToString();
        }
    }
}
