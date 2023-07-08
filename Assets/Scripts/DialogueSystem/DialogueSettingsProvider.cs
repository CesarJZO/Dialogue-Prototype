using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public static class DialogueSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Dialogue System", SettingsScope.Project)
            {
                label = "Dialogue System",
                guiHandler = _ =>
                {
                    SerializedObject settings = DialogueSettings.SerializedSettings;
                    EditorGUILayout.PropertyField(settings.FindProperty("letterDelay"), new GUIContent("Letter Delay"));
                    EditorGUILayout.PropertyField(settings.FindProperty("dialogueDelay"), new GUIContent("Dialogue Delay"));
                    settings.ApplyModifiedProperties();
                },
                keywords = new[] { "Dialogue" }
            };

            return provider;
        }
    }
}
