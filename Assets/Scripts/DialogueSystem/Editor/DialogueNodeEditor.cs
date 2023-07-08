using UnityEditor;

namespace CesarJZO.DialogueSystem.Editor
{
    public class DialogueNodeEditor
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

        public static bool TryRemoveChild(this DialogueNode node, DialogueNode child)
        {

        }
    }
}
