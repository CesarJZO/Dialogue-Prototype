using System;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        public enum Side
        {
            Left,
            Right
        }

        [SerializeField] private Speaker speaker;
        [SerializeField, TextArea] private string text;
        [SerializeField] private Emotion emotion;
        [SerializeField] private Side side;
        [HideInInspector] public Rect rect = new(0f, 0f, 256f, 120f);

        public abstract DialogueNode Child { get; }

        public Speaker Speaker => speaker;

        public Emotion Emotion => emotion;

        public Side PortraitSide => side;

        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set => text = value;
#endif
        }
    }
}
