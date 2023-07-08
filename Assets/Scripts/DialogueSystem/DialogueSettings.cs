using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueSettings : ScriptableObject
    {
        private const string DialogueSettingsPath = "Assets/Editor/DialogueSettings.asset";

        private static DialogueSettings _instance;

        [SerializeField] private float letterDelay;

        [SerializeField] private float dialogueDelay;

        internal static SerializedObject SerializedSettings => new(Instance);

        public static DialogueSettings Instance
        {
            get
            {
                if (_instance) return _instance;

                _instance = AssetDatabase.LoadAssetAtPath<DialogueSettings>(DialogueSettingsPath);

                if (_instance) return _instance;

                _instance = CreateInstance<DialogueSettings>();
                AssetDatabase.CreateAsset(_instance, DialogueSettingsPath);
                AssetDatabase.SaveAssets();

                return _instance;
            }
        }

        public float LetterDelay => letterDelay;

        public float DialogueDelay => dialogueDelay;
    }
}
