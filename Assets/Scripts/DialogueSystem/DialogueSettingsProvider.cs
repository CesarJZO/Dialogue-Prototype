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
                activateHandler = (searchElement, rootElement) =>
                {
                    SerializedObject settings = DialogueSettings.SerializedSettings;



                    settings.ApplyModifiedProperties();
                },
                keywords = new[] { "Dialogue" }
            };

            return provider;
        }
    }
}
