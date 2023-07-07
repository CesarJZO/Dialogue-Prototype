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

        public Emotion Emotion
        {
            get => emotion;
#if UNITY_EDITOR
            set
            {
                emotion = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public PortraitSide PortraitSide
        {
            get => portraitSide;
#if UNITY_EDITOR
            set
            {
                portraitSide = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public Speaker Speaker
        {
            get => speaker;
#if UNITY_EDITOR
            set
            {
                speaker = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

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

#if UNITY_EDITOR
        public abstract bool TryRemoveChild(DialogueNode node);
#endif
    }
}
