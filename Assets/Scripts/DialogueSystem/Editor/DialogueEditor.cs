using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CesarJZO.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private const float CanvasSize = 4000f;
        private const float BackgroundSize = 1024f;

        private static readonly Rect BackgroundCoords = new(0f, 0f, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);

        private Texture2D _backgroundTexture;

        private static Dialogue _selectedDialogueAsset;

        [NonSerialized] private DialogueNode _draggingNode;
        [NonSerialized] private Vector2 _draggingNodeOffset;
        [NonSerialized] private DialogueNode _creatingNode;

        private float _zoom = 1.0f;
        private Vector2 _scrollPosition;
        private Vector3 _canvasOffset;

        private GUIStyle _nodeStyle;

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
            HandleDraggingNode(current);
            if (current.type is EventType.MouseDown && current.button is 1)
            {
                ProcessContextMenu(current.mousePosition);
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Create Node"), false, () => Debug.Log("Hola"));
            menu.ShowAsContext();
        }

        private void HandleDraggingNode(Event current)
        {
            if (current.type is EventType.MouseDown && !_draggingNode && current.button is 0)
            {
                _draggingNode = _selectedDialogueAsset.Nodes.FirstOrDefault(node =>
                    node.rect.Contains(current.mousePosition));
                if (_draggingNode)
                    _draggingNodeOffset = current.mousePosition - _draggingNode.rect.position;
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
        ///
        /// </summary>
        private void DrawNode(DialogueNode node)
        {
            node.rect.height = EditorStyles.textArea.CalcHeight(new GUIContent(node.Text), position.width) + 100f;

            GUILayout.BeginArea(node.rect, _nodeStyle);

            GUILayout.BeginHorizontal();

            GUILayout.Label("Conversant");
            node.Conversant = GUILayout.TextField(node.Conversant, GUILayout.Width(132f));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            node.Text = EditorGUILayout.TextArea(node.Text);
            EditorGUILayout.Space();

            if (GUILayout.Button("Add"))
                _creatingNode = node;

            GUILayout.EndArea();
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

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);

            Rect canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
            GUI.DrawTextureWithTexCoords(
                canvas,
                _backgroundTexture,
                new Rect(0,0,50 * _zoom, 50 * _zoom)
            );

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

            _backgroundTexture = Resources.Load("background_01") as Texture2D;
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
