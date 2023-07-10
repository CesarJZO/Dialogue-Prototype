using UnityEditor;

namespace CesarJZO.DialogueSystem.Editor
{
    public static class SimpleNodeEditor
    {
        public static SerializedProperty FindChildProperty(this SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(SimpleNode.ChildProperty);
        }

        public static void UnlinkNext(this SimpleNode simpleNode, SerializedObject serializedObject)
        {
            SerializedProperty childProperty = serializedObject.FindProperty(SimpleNode.ChildProperty);
            childProperty.objectReferenceValue = null;
        }
    }
}
