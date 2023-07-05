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

        private GUIStyle _simpleNodeStyle;
        private GUIStyle _responseNodeStyle;
        private GUIStyle _itemConditionalNodeStyle;
        private GUIStyle _rootNodeStyle;

        /// <summary>
        ///     Opens the Dialogue Editor window.
        /// </summary>
        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>(
                title: "Dialogue Editor",
                focus: true,
                desiredDockNextTo: typeof(SceneView)
            );
            window.Show();
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
            Event e = Event.current;
            if (e.type is EventType.MouseDown && !_draggingNode && e.button is 0)
            {
                _draggingNode = _selectedDialogueAsset.Nodes.LastOrDefault(node =>
                    node.rect.Contains(e.mousePosition)
                );
                if (_draggingNode)
                {
                    _draggingNodeOffset = e.mousePosition - _draggingNode.rect.position;
                    Selection.activeObject = _draggingNode;
                }
            }
            else if (e.type is EventType.MouseDown && e.button is 1)
            {
                DialogueNode currentNode = _selectedDialogueAsset.Nodes.LastOrDefault(node =>
                    node.rect.Contains(e.mousePosition)
                );
                if (currentNode)
                    HandleRightClickNode(currentNode);
                else
                    ShowAddNodesMenuAsContext(new GenericMenu(), e.mousePosition);
            }
            else if (e.type is EventType.MouseDrag && _draggingNode)
            {
                _draggingNode.rect.position = e.mousePosition - _draggingNodeOffset;
                GUI.changed = true;
            }
            else if (e.type is EventType.MouseUp && e.button is 0)
            {
                _draggingNode = null;
            }
        }

        private void HandleRightClickNode(DialogueNode node)
        {
            var menu = new GenericMenu();
            menu.AddItem(
                content: new GUIContent("Set as Root"),
                on: false,
                func: () => _selectedDialogueAsset.SetNodeAsRoot(node)
            );
            menu.AddItem(
                content: new GUIContent("Delete"),
                on: false,
                func: () => _selectedDialogueAsset.RemoveNode(node)
            );
            menu.ShowAsContext();
        }

        /// <summary>
        ///     Draws a node in the Dialogue Editor window.
        /// </summary>
        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, _simpleNodeStyle);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Speaker:", new GUIStyle(EditorStyles.label) { fixedWidth = 52f });
                    GUILayout.Label(node.Speaker ? node.Speaker.name : "Not Set", EditorStyles.boldLabel);
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();

                node.Text = EditorGUILayout.TextField(node.Text);
                EditorGUILayout.Space();

                if (GUILayout.Button("Add"))
                    _creatingNode = node;
            }
            GUILayout.EndArea();
        }

        /// <summary>
        ///     Draws the connections between nodes in the Dialogue Editor window.
        /// </summary>
        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = node.rect.center + Vector2.right * node.rect.width / 2f;

            // foreach (DialogueNode childNode in node.Children)
            // {
            //     var endPosition = new Vector3
            //     {
            //         x = childNode.rect.xMin,
            //         y = childNode.rect.center.y
            //     };
            //
            //     Vector3 controlOffset = endPosition - startPosition;
            //     controlOffset.y = 0f;
            //     controlOffset.x *= 0.8f;
            //
            //     Handles.DrawBezier(
            //         startPosition,
            //         endPosition,
            //         startPosition + controlOffset,
            //         endPosition - controlOffset,
            //         Color.white,
            //         null,
            //         4f
            //     );
            // }
        }

        #endregion

        private void OnGUI()
        {
            if (!_selectedDialogueAsset)
            {
                DrawLabelAtCenter("No Dialogue Selected");
                return;
            }

            ProcessEvents();

            // Draw a title at the center top of the screen with the name of the selected Dialogue.
            var labelStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = 16
            };

            GUILayout.Label(_selectedDialogueAsset.name, labelStyle);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            // foreach (DialogueNode node in _selectedDialogueAsset.Nodes)
            //     DrawConnections(node);
            foreach (DialogueNode node in _selectedDialogueAsset.Nodes)
                DrawNode(node);

            EditorGUILayout.EndScrollView();
            HandleNodeModifiers();
        }

        private void HandleNodeModifiers()
        {
            if (_creatingNode)
            {
                _selectedDialogueAsset.CreateNodeAtPoint(NodeType.SimpleNode, _creatingNode.rect.position + Vector2.right * 300f);
                _creatingNode = null;
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _simpleNodeStyle = CreateStyle("node0");
            _responseNodeStyle = CreateStyle("node1");
            _itemConditionalNodeStyle = CreateStyle("node2");
            _rootNodeStyle = CreateStyle("node3");

            GUIStyle CreateStyle(string spriteName) => new()
            {
                normal = { background = EditorGUIUtility.Load(spriteName) as Texture2D },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }

        private void ShowAddNodesMenuAsContext(GenericMenu menu, Vector2 position)
        {
            menu.AddItem(
                content: new GUIContent("Add Simple Node"),
                on: false,
                func: () => _selectedDialogueAsset.CreateNodeAtPoint(NodeType.SimpleNode, position)
            );
            menu.AddItem(
                content: new GUIContent("Add Response Node"),
                on: false,
                func: () => _selectedDialogueAsset.CreateNodeAtPoint(NodeType.ResponseNode, position)
            );
            menu.AddItem(
                content: new GUIContent("Add Item Conditional Node"),
                on: false,
                func: () => _selectedDialogueAsset.CreateNodeAtPoint(NodeType.ConditionalNode, position)
            );
            menu.ShowAsContext();
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

        private void DrawLabelAtCenter(string message)
        {
            GUILayout.BeginArea(new Rect(0f, 0f, position.width, position.height));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(message);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }
    }
}
