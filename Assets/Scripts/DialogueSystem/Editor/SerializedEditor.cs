using UnityEditor;

namespace CesarJZO.DialogueSystem.Editor
{
    public static class SerializedEditor
    {
        public static SerializedProperty FindSpeaker(this SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(DialogueNode.SpeakerProperty);
        }

        public static SerializedProperty FindText(this SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(DialogueNode.TextProperty);
        }

        public static SerializedProperty FindEmotion(this SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(DialogueNode.EmotionProperty);
        }

        public static SerializedProperty FindPortraitSide(this SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(DialogueNode.PortraitSideProperty);
        }

        public static SerializedProperty FindHasItem(this SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(ItemConditionalNode.HasItemProperty);
        }
    }
}
