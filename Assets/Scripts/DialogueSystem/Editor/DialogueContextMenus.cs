#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem.Editor
{
    public static class DialogueContextMenus
    {
        public static void AddRightClickNodeItems(this GenericMenu menu, DialogueNode node, DialogueEditorWindow editorWindow)
        {
            menu.AddItem(new GUIContent("Set Node as Root"), false,
                () => editorWindow.SelectedDialogue.SetNodeAsRoot(node)
            );
            menu.AddItem(new GUIContent("Delete Node"), false,
                () => editorWindow.SelectedDialogue.RemoveNode(node)
            );
        }

        public static void AddNodeMenuItems(this GenericMenu menu, Vector2 position, DialogueEditorWindow editorWindow)
        {
            menu.AddItem(new GUIContent("Add Simple Node"), false,
                () => editorWindow.SelectedDialogue.CreateNodeAtPoint(NodeType.SimpleNode, position)
            );
            menu.AddItem(new GUIContent("Add Response Node"), false,
                () => editorWindow.SelectedDialogue.CreateNodeAtPoint(NodeType.ResponseNode, position)
            );
            menu.AddItem(new GUIContent("Add Item Conditional Node"), false,
                () => editorWindow.SelectedDialogue.CreateNodeAtPoint(NodeType.ConditionalNode, position)
            );
            DialogueNode rootNode = editorWindow.SelectedDialogue.RootNode;
            if (rootNode)
                menu.AddItem(new GUIContent("Go To Root Node"), false, () => editorWindow.ScrollToNode(rootNode));
        }

        public static void AddSimpleNodeMenuItems(this GenericMenu menu, SimpleNode simpleNode, DialogueEditorWindow editorWindow)
        {
            menu.AddItem(new GUIContent("Add Node/Add Simple Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToSimpleNode(simpleNode, NodeType.SimpleNode)
            );
            menu.AddItem(new GUIContent("Add Node/Add Conditional Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToSimpleNode(simpleNode, NodeType.ConditionalNode)
            );
            menu.AddItem(new GUIContent("Add Node/Add Response Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToSimpleNode(simpleNode, NodeType.ResponseNode)
            );
            menu.AddItem(new GUIContent("Link Node"), false,
                () => editorWindow.LinkingNode = new NodeContext(simpleNode)
            );
        }

        public static void AddConditionalNodeMenuItems(this GenericMenu menu, ItemConditionalNode conditionalNode, bool which, DialogueEditorWindow editorWindow)
        {
            menu.AddItem(new GUIContent("Add Node/Add Simple Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToConditionalNode(conditionalNode, NodeType.SimpleNode, which)
            );

            menu.AddItem(new GUIContent("Add Node/Add Response Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToConditionalNode(conditionalNode, NodeType.ResponseNode, which)
            );
            menu.AddItem(new GUIContent("Add Node/Add Conditional Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToConditionalNode(conditionalNode, NodeType.ConditionalNode,
                    which)
            );
            menu.AddItem(new GUIContent("Link node"), false,
                () => editorWindow.LinkingNode = new NodeContext(conditionalNode) { valueIfConditional = which }
            );
        }

        public static void AddResponseNodeMenuItems(this GenericMenu menu, ResponseNode responseNode, int index, DialogueEditorWindow editorWindow)
        {
            menu.AddItem(new GUIContent("Add Node/Add Simple Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToResponseNode(responseNode, NodeType.SimpleNode, index)
            );

            menu.AddItem(new GUIContent("Add Node/Add Response Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToResponseNode(responseNode, NodeType.ResponseNode, index)
            );
            menu.AddItem(new GUIContent("Add Node/Add Conditional Node"), false,
                () => editorWindow.SelectedDialogue.AddChildToResponseNode(responseNode, NodeType.ConditionalNode, index)
            );
            menu.AddItem(new GUIContent("Link node"), false,
                () => editorWindow.LinkingNode = new NodeContext(responseNode) { indexIfResponse = index }
            );
        }
    }
}
#endif
