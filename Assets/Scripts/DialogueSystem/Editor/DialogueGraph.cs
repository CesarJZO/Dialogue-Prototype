using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class DialogueGraph : EditorWindow
    {
        private DialogueGraphView _graphView;

        [MenuItem("Window/Dialogue Graph")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueGraph>();
            window.titleContent = new UnityEngine.GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            GenerateToolbar();
            ConstructGraphView();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var nodeCreateButton = new Button(() => _graphView.CreateNode("Dialogue Node"))
            {
                text = "Create Node"
            };

            toolbar.Add(nodeCreateButton);

            rootVisualElement.Add(toolbar);
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView
            {
                name = "Dialogue Graph"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }
    }
}
