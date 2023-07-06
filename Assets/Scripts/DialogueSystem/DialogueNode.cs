using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        [SerializeField] private Speaker speaker;
        [SerializeField] [TextArea] private string text;
        [SerializeField] private Emotion emotion;
        [SerializeField] private PortraitSide portraitSide;

        [HideInInspector, SerializeField] public Rect rect = new(0f, 0f, 256f, 120f);

        public abstract DialogueNode Child { get; }

        public Speaker Speaker => speaker;

        public Emotion Emotion => emotion;

        public PortraitSide PortraitSide => portraitSide;

        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set
            {
                text = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public abstract bool TryRemoveChild(DialogueNode node);
    }
}
