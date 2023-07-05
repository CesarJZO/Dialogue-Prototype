using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        [SerializeField] private Speaker speaker;
        [SerializeField, TextArea] private string text;
        [SerializeField] private Emotion emotion;
        [SerializeField] private PortraitSide portraitSide;

        public NodeType Type => GetType().Name switch
        {
            "ItemConditionalNode" => NodeType.ConditionalNode,
            "ResponseNode" => NodeType.ResponseNode,
            _ => NodeType.SimpleNode
        };

        // [HideInInspector]
        public Rect rect = new(0f, 0f, 256f, 120f);

        public abstract DialogueNode Child { get; }

        public Speaker Speaker => speaker;

        public Emotion Emotion => emotion;

        public PortraitSide PortraitSide => portraitSide;

        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set => text = value;
#endif
        }

        public abstract bool IsChild(DialogueNode node);

        public abstract void RemoveChild(DialogueNode node);
    }
}
