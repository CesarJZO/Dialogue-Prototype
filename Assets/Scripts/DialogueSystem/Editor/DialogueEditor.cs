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
        [NonSerialized] private NodeContext _creatingNode;
        [NonSerialized] private NodeContext _linkingNode;
        [NonSerialized] private Vector2 _draggingNodeOffset;
        [NonSerialized] private Vector2 _scrollPosition;

        private GUIStyle _selectedNodeStyle;
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
                    if (_linkingNode is not null)
                    {
                        if (_linkingNode.parentNode is SimpleNode simpleNode)
                            simpleNode.SetChild(_draggingNode);
                        else if (_linkingNode.parentNode is ResponseNode responseNode)
                            responseNode.SetChild(_draggingNode, _linkingNode.indexIfResponse);
                        else if (_linkingNode.parentNode is ItemConditionalNode itemConditionalNode)
                            itemConditionalNode.SetChild(_draggingNode, _linkingNode.valueIfConditional);
                        // EditorUtility.SetDirty(_selectedDialogueAsset);
                        _linkingNode = null;
                    }
                    _draggingNodeOffset = e.mousePosition - _draggingNode.rect.position;
                    Selection.activeObject = _draggingNode;
                }
                else
                {
                    Selection.activeObject = _selectedDialogueAsset;
                }
            }
            else if (e.type is EventType.MouseDown && e.button is 1)
            {
                DialogueNode currentNode = _selectedDialogueAsset.Nodes.LastOrDefault(node =>
                    node.rect.Contains(e.mousePosition)
                );
                if (currentNode)
                {
                    GenericMenu menu = RightClickNodeMenuBase(currentNode);
                    if (currentNode is SimpleNode simpleNode)
                    {
                        AddSimpleNodeMenuItems(simpleNode, ref menu);
                    }
                    menu.ShowAsContext();
                }
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

            if (e.type is EventType.KeyUp && e.keyCode is KeyCode.Escape && _linkingNode is not null)
            {
                _linkingNode = null;
            }
        }

        private GenericMenu RightClickNodeMenuBase(DialogueNode node)
        {
            var menu = new GenericMenu();
            menu.AddItem(
                content: new GUIContent("Set Node as Root"),
                on: false,
                func: () => _selectedDialogueAsset.SetNodeAsRoot(node)
            );
            menu.AddItem(
                content: new GUIContent("Delete Node"),
                on: false,
                func: () => _selectedDialogueAsset.RemoveNode(node)
            );
            menu.ShowAsContext();

            return menu;
        }

        /// <summary>
        ///     Draws a node in the Dialogue Editor window.
        /// </summary>
        private void DrawNode(DialogueNode node)
        {
            node.rect.height = GetHeightForNode(node);

            GUILayout.BeginArea(node.rect, node.Type switch
            {
                _ when node == Selection.activeObject => _selectedNodeStyle,
                _ when _selectedDialogueAsset.IsRoot(node) => _rootNodeStyle,
                NodeType.ResponseNode => _responseNodeStyle,
                NodeType.ConditionalNode => _itemConditionalNodeStyle,
                _ => _simpleNodeStyle
            });
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

                if (node is ItemConditionalNode conditionalNode)
                    DrawConditionalNode(conditionalNode);
                else if (node is ResponseNode responseNode)
                    DrawResponseNode(responseNode);
            }
            GUILayout.EndArea();
        }

        private void AddSimpleNodeMenuItems(SimpleNode simpleNode, ref GenericMenu menu)
        {
            if (!simpleNode.Child)
            {
                menu.AddItem(new GUIContent("Add Node/Add Simple Node"), false,
                    () => _selectedDialogueAsset.AddChildToSimpleNode(simpleNode, NodeType.SimpleNode)
                );
                menu.AddItem(new GUIContent("Add Node/Add Conditional Node"), false,
                    () => _selectedDialogueAsset.AddChildToSimpleNode(simpleNode, NodeType.ConditionalNode)
                );
                menu.AddItem(new GUIContent("Add Node/Add Response Node"), false,
                    () => _selectedDialogueAsset.AddChildToSimpleNode(simpleNode, NodeType.ResponseNode)
                );
                menu.AddItem(new GUIContent("Link Node"), false,
                    () => _linkingNode = new NodeContext(simpleNode)
                );
            }
            else
            {
                menu.AddItem(new GUIContent("Unlink Child"), false, simpleNode.UnlinkChild);
            }
        }

        private void AddConditionalNodeMenuItems(ItemConditionalNode conditionalNode, ref GenericMenu menu, bool which)
        {
            menu.AddItem(new GUIContent("Add Node/Add Simple Node"), false,
                () => _selectedDialogueAsset.AddChildToConditionalNode(conditionalNode, NodeType.SimpleNode, which)
            );

            menu.AddItem(new GUIContent("Add Node/Add Response Node"), false,
                () => _selectedDialogueAsset.AddChildToConditionalNode(conditionalNode, NodeType.ResponseNode, which)
            );
            menu.AddItem(new GUIContent("Add Node/Add Conditional Node"), false,
                () => _selectedDialogueAsset.AddChildToConditionalNode(conditionalNode, NodeType.ConditionalNode, which)
            );
            menu.AddItem(new GUIContent("Link node"), false,
                () => _linkingNode = new NodeContext(conditionalNode) { valueIfConditional = which }
            );
        }

        private void AddResponseNodeMenuItems(ResponseNode responseNode, ref GenericMenu menu, int index)
        {
            menu.AddItem(new GUIContent("Add Node/Add Simple Node"), false,
                () => _selectedDialogueAsset.AddChildToResponseNode(responseNode, NodeType.SimpleNode, index)
            );

            menu.AddItem(new GUIContent("Add Node/Add Response Node"), false,
                () => _selectedDialogueAsset.AddChildToResponseNode(responseNode, NodeType.ResponseNode, index)
            );
            menu.AddItem(new GUIContent("Add Node/Add Conditional Node"), false,
                () => _selectedDialogueAsset.AddChildToResponseNode(responseNode, NodeType.ConditionalNode, index)
            );
            menu.AddItem(new GUIContent("Link node"), false,
                () => _linkingNode = new NodeContext(responseNode) { indexIfResponse = index }
            );
        }

        private void DrawConditionalNode(ItemConditionalNode conditionalNode)
        {
            const float buttonWidth = 64f;
            DrawGUIElements(true);
            DrawGUIElements(false);

            void DrawGUIElements(bool which)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"Child If {which}:");
                    bool hasChild = conditionalNode.GetChild(which);
                    if (GUILayout.Button(hasChild ? "Unlink" : "Add", GUILayout.Width(buttonWidth)))
                    {
                        if (!hasChild)
                        {
                            var menu = new GenericMenu();
                            AddConditionalNodeMenuItems(conditionalNode, ref menu, which);
                            menu.ShowAsContext();
                        }
                        else
                        {
                            conditionalNode.UnlinkChild(which);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawResponseNode(ResponseNode responseNode)
        {
            const float buttonWidth = 64f;

            for (int i = 0; i < responseNode.ChildrenCount; i++)
                DrawResponse(i);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add Response"))
                    responseNode.AddResponse();
                if (GUILayout.Button("Remove Response"))
                    responseNode.RemoveResponse();
            }
            GUILayout.EndHorizontal();

            void DrawResponse(int index)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"Response {index + 1}");
                    bool hasChild = responseNode.GetChild(index);
                    if (GUILayout.Button(hasChild ? "Unlink" : "Add", GUILayout.Width(buttonWidth)))
                    {
                        if (!hasChild)
                        {
                            var menu = new GenericMenu();
                            AddResponseNodeMenuItems(responseNode, ref menu, index);
                            menu.ShowAsContext();
                        }
                        else
                        {
                            responseNode.UnlinkChild(index);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///     Draws the connections between nodes in the Dialogue Editor window.
        /// </summary>
        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = node.rect.center + Vector2.right * node.rect.width / 2f;

            if (_linkingNode is not null && _linkingNode.parentNode == node)
            {
                Handles.DrawBezier(
                    startPosition,
                    Event.current.mousePosition,
                    startPosition + Vector3.right * 50f,
                    Event.current.mousePosition - Vector2.right * 50f,
                    Color.white,
                    null,
                    4f
                );
                GUI.changed = true;
                return;
            }

            DialogueNode childNode = node.Child;

            if (!childNode) return;

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
                // alignment = TextAnchor.UpperCenter,
                fontSize = 16
            };

            GUILayout.Label(_selectedDialogueAsset.name, labelStyle);

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
            if (_creatingNode?.parentNode)
            {
                _selectedDialogueAsset.CreateNodeAtPoint(NodeType.ConditionalNode,
                    _creatingNode.parentNode.rect.position + Vector2.right * 300f);
                _creatingNode = null;
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _selectedNodeStyle = CreateStyle("node1");
            _simpleNodeStyle = CreateStyle("node0");
            _responseNodeStyle = CreateStyle("node0");
            _itemConditionalNodeStyle = CreateStyle("node0");
            _rootNodeStyle = CreateStyle("node5");

            GUIStyle CreateStyle(string path) => new()
            {
                normal = { background = EditorGUIUtility.Load(path) as Texture2D },
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

        private float GetHeightForNode(DialogueNode node)
        {
            return node switch
            {
                ItemConditionalNode => 138f,
                ResponseNode responseNode => 124f + responseNode.ChildrenCount * 20f,
                _ => 92f
            };
        }
    }

    public class NodeContext
    {
        public readonly DialogueNode parentNode;
        public bool valueIfConditional;
        public int indexIfResponse;

        public NodeContext(DialogueNode parentNode)
        {
            this.parentNode = parentNode;
        }
    }
}
