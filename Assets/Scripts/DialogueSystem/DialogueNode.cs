using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        [SerializeField] private Speaker speaker;
        [SerializeField] [TextArea] private string text;
        [SerializeField] private SpeakerEmotion emotion;
        [SerializeField] private PortraitSide portraitSide;

        [HideInInspector, SerializeField] public Rect rect = new(0f, 0f, 256f, 120f);

        public Speaker Speaker => speaker;

        public string Text => text;

        public SpeakerEmotion Emotion => emotion;

        public PortraitSide PortraitSide => portraitSide;

        public abstract DialogueNode Child { get; }


#if UNITY_EDITOR
        public abstract bool TryRemoveChild(DialogueNode node);
#endif

        public const string SpeakerProperty = nameof(speaker);
        public const string TextProperty = nameof(text);
        public const string EmotionProperty = nameof(emotion);
        public const string PortraitSideProperty = nameof(portraitSide);
    }
}
