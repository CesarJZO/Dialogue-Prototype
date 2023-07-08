using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

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

                    var title = new Label("Dialogue Settings");
                    title.AddToClassList("title");
                    rootElement.Add(title);

                    var properties = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Column
                        }
                    };
                    properties.AddToClassList("property-list");
                    rootElement.Add(properties);

                    var letterDelayTextField = new FloatField("Letter Delay")
                    {
                        value = settings.FindProperty("letterDelay").floatValue
                    };
                    letterDelayTextField.AddToClassList("letter-delay-field");
                    properties.Add(letterDelayTextField);

                    var dialogueDelayTextField = new FloatField("Dialogue Delay")
                    {
                        value = settings.FindProperty("dialogueDelay").floatValue
                    };
                    dialogueDelayTextField.AddToClassList("dialogue-delay-field");
                    properties.Add(dialogueDelayTextField);

                    letterDelayTextField.RegisterValueChangedCallback(evt =>
                    {
                        settings.FindProperty("letterDelay").floatValue = evt.newValue;
                        settings.ApplyModifiedProperties();
                    });

                    dialogueDelayTextField.RegisterValueChangedCallback(evt =>
                    {
                        settings.FindProperty("dialogueDelay").floatValue = evt.newValue;
                        settings.ApplyModifiedProperties();
                    });

                    settings.ApplyModifiedProperties();
                },
                keywords = new[] { "Dialogue" }
            };

            return provider;
        }
    }
}
