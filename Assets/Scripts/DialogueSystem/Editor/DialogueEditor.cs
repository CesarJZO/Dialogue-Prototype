using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CesarJZO.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static Dialogue _selectedDialogueAsset;

        [NonSerialized] private static DialogueNode _creatingNode;

        private Vector2 _scrollPosition;

        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>(
                "Dialogue Editor",
                typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow")
            );
            window.Show();
            window.Focus();
        }

        [OnOpenAsset(1)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            _selectedDialogueAsset = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (!_selectedDialogueAsset) return false;
            ShowWindow();
            return true;
        }

        #region Dialogue GUI

        private void DrawNode(DialogueNode node)
        {
            if (GUILayout.Button($"Add to node {node.name}"))
                _creatingNode = node;
        }

        #endregion

        private void OnGUI()
        {
            if (!_selectedDialogueAsset)
            {
                EditorGUILayout.LabelField("No dialogue selected.");
                return;
            }

            // Add a scroll view
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            foreach (DialogueNode node in _selectedDialogueAsset)
            {
                DrawNode(node);
            }

            if (_creatingNode)
            {
                _selectedDialogueAsset.CreateNode(_creatingNode);
                _creatingNode = null;
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            var selected = Selection.activeObject as Dialogue;
            if (selected)
                _selectedDialogueAsset = selected;
            Repaint();
        }
    }
}
