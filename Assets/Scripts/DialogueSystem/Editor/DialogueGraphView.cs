using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class DialogueGraphView : GraphView
    {
        private readonly Vector2 _defaultNodeSize = new Vector2(150, 200);

        public DialogueGraphView()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            DialogueNode entryPoint = GenerateEntryPoint();
            AddElement(entryPoint);
        }

        private DialogueNode GenerateEntryPoint()
        {
            var node = new DialogueNode
            {
                title = "START",
                guid = Guid.NewGuid().ToString(),
                dialogueText = "BANANA",
                entryPoint = true
            };

            Port port = GeneratePort(node, Direction.Output);
            port.portName = "Next"; // Used for saving

            node.outputContainer.Add(port);

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(100, 200, 100, 150));

            return node;
        }

        private Port GeneratePort(DialogueNode targetNode, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return targetNode.InstantiatePort(
                orientation: Orientation.Horizontal,
                direction: portDirection,
                capacity: capacity,
                type: typeof(float) // Type is arbitrary because it is not used
            );
        }

        public void CreateNode(string nodeName)
        {
            AddElement(CreateDialogueNode(nodeName));
        }

        private DialogueNode CreateDialogueNode(string nodeName)
        {
            var dialogueNode = new DialogueNode
            {
                title = nodeName,
                dialogueText = nodeName,
                guid = Guid.NewGuid().ToString()
            };

            Port inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";

            dialogueNode.inputContainer.Add(inputPort);

            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
            dialogueNode.SetPosition(new Rect(Vector2.zero, _defaultNodeSize));

            return dialogueNode;
        }
    }
}
