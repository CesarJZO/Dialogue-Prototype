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

#if UNITY_EDITOR
        internal static SerializedObject SerializedSettings => new(Instance);
#endif

        public static DialogueSettings Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance) return _instance;

                _instance = AssetDatabase.LoadAssetAtPath<DialogueSettings>(DialogueSettingsPath);

                if (_instance) return _instance;

                _instance = CreateInstance<DialogueSettings>();
                AssetDatabase.CreateAsset(_instance, DialogueSettingsPath);
                AssetDatabase.SaveAssets();
#endif
                return _instance;
            }
        }

        public float LetterDelay => letterDelay;

        public float DialogueDelay => dialogueDelay;
    }
}
