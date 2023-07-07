using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueSettings : ScriptableObject
    {
        public const string DialogueSettingsPath = "Assets/Editor/DialogueSettings.asset";

        [SerializeField] private float letterDelay;

        public float LetterDelay => letterDelay;

        [SerializeField] private float dialogueDelay;

        public float DialogueDelay => dialogueDelay;

        private static DialogueSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<DialogueSettings>(DialogueSettingsPath);

            if (settings) return settings;

            settings = CreateInstance<DialogueSettings>();
            AssetDatabase.CreateAsset(settings, DialogueSettingsPath);
            AssetDatabase.SaveAssets();

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}
