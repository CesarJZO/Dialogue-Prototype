using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CesarJZO.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static Dialogue _selectedDialogueAsset;

        [NonSerialized] private DialogueNode _draggingNode;
        [NonSerialized] private Vector2 _draggingNodeOffset;
        [NonSerialized] private DialogueNode _creatingNode;

        private Vector2 _scrollPosition;

        private GUIStyle _nodeStyle;
        private GUIStyle _responseNodeStyle;

        /// <summary>
        ///     Opens the Dialogue Editor window.
        /// </summary>
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

        private void ProcessEvents()
        {
            Event current = Event.current;
            if (current.type is EventType.MouseDown && !_draggingNode && current.button is 0)
            {
                _draggingNode = _selectedDialogueAsset.Nodes.FirstOrDefault(node =>
                    node.rect.Contains(current.mousePosition));
                if (_draggingNode)
                {
                    _draggingNodeOffset = current.mousePosition - _draggingNode.rect.position;
                    // Selection.activeObject = _draggingNode;
                }
            }
            else if (current.type is EventType.MouseDrag && _draggingNode)
            {
                _draggingNode.rect.position = current.mousePosition - _draggingNodeOffset;
                GUI.changed = true;
            }
            else if (current.type is EventType.MouseUp && current.button is 0)
            {
                _draggingNode = null;
            }
        }

        /// <summary>
        ///     Draws a node in the Dialogue Editor window.
        /// </summary>
        private void DrawNode(DialogueNode node)
        {
            float rectHeight = EditorStyles.textArea.CalcHeight(new GUIContent(node.Text), position.width) +
                               (IsResponse() ? 100f : 128f);

            node.rect.height = rectHeight;

            GUILayout.BeginArea(node.rect, IsResponse() ? _responseNodeStyle : _nodeStyle);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Speaker");

            node.Conversant = EditorGUILayout.TextField(node.Conversant, GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            node.Text = EditorGUILayout.TextArea(node.Text);
            EditorGUILayout.Space();

            if (!IsResponse())
            {
                node.ChildrenAreResponses = EditorGUILayout.Toggle(
                    $"Children are{(node.ChildrenAreResponses ? " " : " not ")}responses",
                    node.ChildrenAreResponses
                );
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add"))
                _creatingNode = node;

            GUILayout.EndArea();

            bool IsResponse() => node.Parent && node.Parent.ChildrenAreResponses;
        }

        /// <summary>
        ///     Draws the connections between nodes in the Dialogue Editor window.
        /// </summary>
        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = node.rect.center + Vector2.right * node.rect.width / 2f;

            foreach (DialogueNode childNode in node.Children)
            {
                var endPosition = new Vector3
                {
                    x = childNode.rect.xMin,
                    y = childNode.rect.center.y
                };

                Vector3 controlOffset = endPosition - startPosition;
                controlOffset.y = 0f;
                controlOffset.x *= 0.8f;

                Handles.DrawBezier(
                    startPosition,
                    endPosition,
                    startPosition + controlOffset,
                    endPosition - controlOffset,
                    Color.white,
                    null,
                    4f
                );
            }
        }

        #endregion

        private void OnGUI()
        {
            if (!_selectedDialogueAsset)
            {
                EditorGUILayout.LabelField("No dialogue selected.");
                return;
            }

            ProcessEvents();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            foreach (DialogueNode node in _selectedDialogueAsset.Nodes)
                DrawConnections(node);
            foreach (DialogueNode node in _selectedDialogueAsset.Nodes)
                DrawNode(node);

            EditorGUILayout.EndScrollView();
            HandleNodeModifiers();
        }

        private void HandleNodeModifiers()
        {
            if (_creatingNode)
            {
                _selectedDialogueAsset.CreateNode(_creatingNode);
                _creatingNode = null;
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _nodeStyle = new GUIStyle
            {
                normal = { background = EditorGUIUtility.Load("node0") as Texture2D },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };

            _responseNodeStyle = new GUIStyle
            {
                normal = { background = EditorGUIUtility.Load("node1") as Texture2D },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
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
