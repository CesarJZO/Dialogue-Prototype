#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using CesarJZO.InventorySystem;

namespace CesarJZO.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private const float CanvasSize = 10_000f;
        private const float BackgroundSize = 64f;
        private const float BackgroundLength = CanvasSize / BackgroundSize;

        private static readonly Rect BackgroundCoords = new(0f, 0f, BackgroundLength, BackgroundLength);
        private static readonly string[] Sides = { "Left", "Right" };
        private static readonly string[] Emotions = { "Neutral", "Happy", "Sad", "Angry" };

        private static DialogueEditor _editor;

        [SerializeField] private Vector2 scrollPosition;
        [SerializeField] private Dialogue selectedDialogue;

        [NonSerialized] private Texture2D _backgroundTexture;

        [NonSerialized] private bool _draggingCanvas;
        [NonSerialized] private Vector2 _draggingCanvasOffset;

        [NonSerialized] private DialogueNode _draggingNode;
        [NonSerialized] private Vector2 _draggingNodeOffset;

        [NonSerialized] private GUIStyle _selectedNodeStyle;
        [NonSerialized] private GUIStyle _rootNodeStyle;
        [NonSerialized] private GUIStyle _simpleNodeStyle;
        [NonSerialized] private GUIStyle _responseNodeStyle;
        [NonSerialized] private GUIStyle _itemConditionalNodeStyle;

        public Dialogue SelectedDialogue => selectedDialogue;

        [field: NonSerialized] public NodeContext LinkingNode { get; set; }

        /// <summary>
        ///     Opens the Dialogue Editor window.
        /// </summary>
        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            _editor = GetWindow<DialogueEditor>(
                title: "Dialogue Editor",
                focus: true,
                desiredDockNextTo: typeof(SceneView)
            );
        }

        [OnOpenAsset(1)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            var selected = EditorUtility.InstanceIDToObject(instanceID);

            if (selected is not (Dialogue or DialogueNode)) return false;

            ShowWindow();

            string path = AssetDatabase.GetAssetPath(instanceID);
            _editor.selectedDialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(path);

            if (selected is DialogueNode node)
            {
                _editor.ScrollToNode(node);
            }
            else
            {
                if (_editor.selectedDialogue.RootNode)
                    _editor.ScrollToNode(_editor.selectedDialogue.RootNode);
            }

            return true;
        }

        private void Awake()
        {
            _editor = this;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _selectedNodeStyle = CreateStyle("node1");
            _simpleNodeStyle = CreateStyle("node0");
            _responseNodeStyle = CreateStyle("node0");
            _itemConditionalNodeStyle = CreateStyle("node0");
            _rootNodeStyle = CreateStyle("node5");

            _backgroundTexture = Resources.Load<Texture2D>("background");

            if (selectedDialogue && selectedDialogue.RootNode)
                ScrollToNode(selectedDialogue.RootNode);
            else
                scrollPosition = new Vector2(0f, CanvasSize / 2f);

            GUIStyle CreateStyle(string path) => new()
            {
                normal = { background = EditorGUIUtility.Load(path) as Texture2D },
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        private void OnGUI()
        {
            if (selectedDialogue)
                ProcessEventsOnAsset();

            ProcessScrollView();

            string assetName = selectedDialogue ? selectedDialogue.name : "None";
            GUILayout.Label($"Editing: {assetName}", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);
            {
                Rect canvasRect = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                GUI.DrawTextureWithTexCoords(canvasRect, _backgroundTexture, BackgroundCoords);
                if (selectedDialogue)
                {
                    foreach (DialogueNode node in selectedDialogue.Nodes)
                        DrawConnections(node);
                    foreach (DialogueNode node in selectedDialogue.Nodes)
                        DrawNode(node);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnSelectionChanged()
        {
            var selected = Selection.activeObject;

            if (selected is not (Dialogue or DialogueNode)) return;

            string path = AssetDatabase.GetAssetPath(selected);
            selectedDialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(path);

            if (_editor)
                _editor.Focus();

            Repaint();
        }

        private static Vector2 GetSizeForNode(DialogueNode node)
        {
            const float testHeight = 92f;
            return node switch
            {
                ItemConditionalNode => new Vector2(256f, 152f + testHeight),
                ResponseNode responseNode => new Vector2(256f, 124f + responseNode.ChildrenCount * 20f + testHeight + 24f),
                _ => new Vector2(256f, 116f + testHeight)
            };
        }

        public void ScrollToNode(DialogueNode node)
        {
            float horizontalOffset = selectedDialogue.IsRoot(node) ? 64f : position.width / 2f - node.rect.width / 2f;

            scrollPosition = node.rect.position - new Vector2(
                horizontalOffset, position.height / 2f - node.rect.height / 2f
            );
        }

        private void ProcessEventsOnAsset()
        {
            Event e = Event.current;
            if (e.type is EventType.MouseDown && !_draggingNode && e.button is 0)
            {
                _draggingNode = selectedDialogue.Nodes.LastOrDefault(node =>
                    node.rect.Contains(e.mousePosition + scrollPosition)
                );
                if (_draggingNode)
                {
                    _draggingNodeOffset = e.mousePosition - _draggingNode.rect.position;

                    if (LinkingNode is not null)
                    {
                        if (LinkingNode.parentNode is SimpleNode simpleNode)
                            simpleNode.SetChild(_draggingNode);
                        else if (LinkingNode.parentNode is ResponseNode responseNode)
                            responseNode.SetChild(_draggingNode, LinkingNode.indexIfResponse);
                        else if (LinkingNode.parentNode is ItemConditionalNode itemConditionalNode)
                            itemConditionalNode.SetChild(_draggingNode, LinkingNode.valueIfConditional);
                        LinkingNode = null;
                    }

                    Selection.activeObject = _draggingNode;
                }
                else
                {
                    if (LinkingNode is not null)
                    {
                        LinkingNode = null;
                        return;
                    }

                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (e.type is EventType.MouseDown && e.button is 1)
            {
                if (LinkingNode is not null)
                {
                    LinkingNode = null;
                    return;
                }

                DialogueNode currentNode = selectedDialogue.Nodes.LastOrDefault(node =>
                    node.rect.Contains(e.mousePosition + scrollPosition)
                );
                if (currentNode)
                {
                    var menu = new GenericMenu();
                    menu.AddRightClickNodeItems(currentNode, this);
                    menu.ShowAsContext();
                }
                else
                {
                    var menu = new GenericMenu();
                    menu.AddNodeMenuItems(e.mousePosition + scrollPosition, this);
                    menu.ShowAsContext();
                }
            }
            else if (e.type is EventType.MouseDrag && e.button is 0 && _draggingNode)
            {
                _draggingNode.rect.position = e.mousePosition - _draggingNodeOffset;
                GUI.changed = true;
            }
            else if (e.type is EventType.MouseUp && e.button is 0)
            {
                _draggingNode = null;
            }

            if (e.type is EventType.KeyUp && e.keyCode is KeyCode.Escape && LinkingNode is not null)
                LinkingNode = null;
        }

        private void ProcessScrollView()
        {
            Event e = Event.current;
            if (e.type is EventType.MouseDown && e.button is 2)
            {
                _draggingCanvas = true;
                _draggingCanvasOffset = e.mousePosition + scrollPosition;
            }
            else if (e.type is EventType.MouseUp && e.button is 2)
            {
                _draggingCanvas = false;
            }
            else if (e.type is EventType.MouseDrag && e.button is 2 && _draggingCanvas)
            {
                scrollPosition = _draggingCanvasOffset - e.mousePosition;
                GUI.changed = true;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            node.rect.size = GetSizeForNode(node);

            GUILayout.BeginArea(node.rect, node switch
            {
                _ when node == Selection.activeObject => _selectedNodeStyle,
                _ when selectedDialogue.IsRoot(node) => _rootNodeStyle,
                ResponseNode => _responseNodeStyle,
                ItemConditionalNode => _itemConditionalNodeStyle,
                _ => _simpleNodeStyle
            });
            {
                GUILayout.Label("Speaker");
                node.Speaker = EditorGUILayout.ObjectField(node.Speaker, typeof(Speaker), false) as Speaker;
                EditorGUILayout.Space();

                GUILayout.Label("Emotion & Side");
                GUILayout.BeginHorizontal();
                {
                    node.Emotion = (Emotion) EditorGUILayout.Popup((int)node.Emotion, Emotions);
                    node.PortraitSide = (PortraitSide) EditorGUILayout.Popup((int)node.PortraitSide, Sides);
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();

                GUILayout.Label("Text");
                node.Text = EditorGUILayout.TextField(node.Text);
                EditorGUILayout.Space();

                if (node is SimpleNode simpleNode)
                    DrawSimpleNode(simpleNode);
                else if (node is ItemConditionalNode conditionalNode)
                    DrawConditionalNode(conditionalNode);
                else if (node is ResponseNode responseNode)
                    DrawResponseNode(responseNode);
            }
            GUILayout.EndArea();
        }

        private void DrawSimpleNode(SimpleNode simpleNode)
        {
            if (simpleNode.GetChild())
            {
                if (GUILayout.Button("Unlink"))
                    simpleNode.UnlinkChild();
            }
            else
            {
                if (GUILayout.Button("Add"))
                {
                    var menu = new GenericMenu();
                    menu.AddSimpleNodeMenuItems(simpleNode, this);
                    menu.ShowAsContext();
                }
            }
        }

        private void DrawConditionalNode(ItemConditionalNode conditionalNode)
        {
            const float buttonWidth = 64f;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Has Item", new GUIStyle(EditorStyles.label) { fixedWidth = 54f });
                conditionalNode.Item = EditorGUILayout.ObjectField(conditionalNode.Item, typeof(Item), false) as Item;
            }
            GUILayout.EndHorizontal();

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
                            menu.AddConditionalNodeMenuItems(conditionalNode, which, this);
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

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Limit (s)", new GUIStyle(EditorStyles.label) { fixedWidth = 54f });
                responseNode.TimeLimit = EditorGUILayout.FloatField(Mathf.Clamp(responseNode.TimeLimit, 0f, float.MaxValue));
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            for (var i = 0; i < responseNode.ChildrenCount; i++)
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
                    responseNode.SetText(
                        GUILayout.TextField(responseNode.GetText(index), GUILayout.Width(150f)),
                        index
                    );

                    bool hasChild = responseNode.GetChild(index);
                    if (GUILayout.Button(hasChild ? "Unlink" : "Add", GUILayout.Width(buttonWidth)))
                    {
                        if (!hasChild)
                        {
                            var menu = new GenericMenu();
                            menu.AddResponseNodeMenuItems(responseNode, index, this);
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

        private void DrawConnections(DialogueNode node)
        {
            Vector2 start;
            Vector2 end;

            if (LinkingNode is not null && LinkingNode.parentNode == node)
            {
                start = node switch
                {
                    ItemConditionalNode => GetStartPointForConditionalNode(LinkingNode.valueIfConditional),
                    ResponseNode => GetStartPointForResponseNode(LinkingNode.indexIfResponse),
                    _ => GetStartPointDefault()
                };
                end = Event.current.mousePosition;
                DrawCurve(start, end);
                GUI.changed = true;
            }

            if (node is ItemConditionalNode conditionalNode)
            {
                DialogueNode trueChild = conditionalNode.GetChild(true);
                if (trueChild)
                {
                    start = GetStartPointForConditionalNode(true);
                    end = trueChild.rect.center + Vector2.left * trueChild.rect.width / 2f;
                    DrawCurve(start, end);
                }

                DialogueNode falseChild = conditionalNode.GetChild(false);
                if (falseChild)
                {
                    start = GetStartPointForConditionalNode(false);
                    end = falseChild.rect.center + Vector2.left * falseChild.rect.width / 2f;
                    DrawCurve(start, end);
                }
            }
            else if (node is ResponseNode responseNode)
            {
                for (var i = 0; i < responseNode.ChildrenCount; i++)
                {
                    start = GetStartPointForResponseNode(i);
                    DialogueNode child = responseNode.GetChild(i);
                    if (!child) continue;
                    end = child.rect.center + Vector2.left * child.rect.width / 2f;
                    DrawCurve(start, end);
                }
            }
            else
            {
                DialogueNode child = node.Child;
                if (!child) return;

                start = node.rect.center + Vector2.right * node.rect.width / 2f;
                end = child.rect.center + Vector2.left * child.rect.width / 2f;
                DrawCurve(start, end);
            }

            void DrawCurve(Vector3 startPos, Vector3 endPos)
            {
                Vector3 controlOffset = endPos - startPos;
                controlOffset.y = 0f;
                controlOffset.x *= 0.8f;

                Handles.DrawBezier(
                    startPosition: startPos,
                    endPosition: endPos,
                    startTangent: startPos + controlOffset,
                    endTangent: endPos - controlOffset,
                    color: Color.white,
                    texture: null,
                    width: 4f
                );
            }

            Vector2 GetStartPointDefault()
            {
                return node.rect.center + Vector2.right * node.rect.width / 2f;
            }

            Vector2 GetStartPointForConditionalNode(bool which)
            {
                return node.rect.position + new Vector2
                {
                    x = node.rect.width,
                    y = (which ? 100f : 120f) + 90f
                };
            }

            Vector2 GetStartPointForResponseNode(int index)
            {
                return node.rect.position + new Vector2
                {
                    x = node.rect.width,
                    y = 84f + 88f + index * 20f + 24f
                };
            }
        }
    }
}
#endif
