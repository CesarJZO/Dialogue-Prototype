using System;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        /// <summary>
        ///     Invoked when <see cref="Next"/> is called, sending the previous node.
        /// </summary>
        public event Action<DialogueNode> NodeChanged;

        public event Action ConversationStarted;
        public event Action ConversationEnded;
        public event Action ConversationUpdated;

        public static DialogueManager Instance { get; private set; }

        /// <summary>
        ///     Whether there is a dialogue currently in progress.
        /// </summary>
        public bool HasDialogue => _currentDialogue;

        /// <summary>
        ///     The speaker of the current node.
        /// </summary>
        public Speaker CurrentSpeaker => _currentNode.Speaker;

        /// <summary>
        ///     The side of the dialogue UI where the current speaker should be displayed.
        /// </summary>
        public DialogueNode.Side CurrentSide => _currentNode.PortraitSide;

        public Emotion CurrentEmotion => _currentNode.Emotion;

        /// <summary>
        ///     The text of the current node.
        /// </summary>
        public string CurrentText => _currentNode.Text;

        /// <summary>
        ///     Whether the current node is a <see cref="ResponseNode"/> or an <see cref="ItemConditionalNode"/>.
        /// </summary>
        public bool Prompting => _currentNode is ResponseNode or ItemConditionalNode;

        /// <summary>
        ///     The current node.
        /// </summary>
        public DialogueNode CurrentNode => _currentNode;

        /// <summary>
        ///     The child node of the current node. If the current node is
        ///     a <see cref="ItemConditionalNode"/>, returns the child of the
        ///     <see cref="ItemConditionalNode.Child"/>.
        /// </summary>
        public bool HasNextNode => _currentNode.Child;

        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        ///     Sets current node and dialogue, and invokes <see cref="ConversationUpdated"/>.
        /// </summary>
        /// <param name="dialogue">The dialogue to start.</param>
        public void StartDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentNode = _currentDialogue.RootNode;
            ConversationStarted?.Invoke();
            ConversationUpdated?.Invoke();
        }

        /// <summary>
        ///     Finishes the current conversation and invokes <see cref="ConversationUpdated"/> and <see cref="ConversationEnded"/>.
        /// </summary>
        public void Quit()
        {
            _currentDialogue = null;
            _currentNode = null;
            ConversationUpdated?.Invoke();
            ConversationEnded?.Invoke();
        }

        /// <summary>
        ///     Sets the current node to the next node and invokes <see cref="ConversationUpdated"/>.
        ///     Also, calls <see cref="Quit"/> if there is no next node.
        /// </summary>
        public void Next()
        {
            if (!HasNextNode)
                throw new DialogueNoNextNodeException("There is no next node.");

            DialogueNode parent = _currentNode;

            _currentNode = _currentNode.Child;

            NodeChanged?.Invoke(parent);

            ConversationUpdated?.Invoke();
        }
    }
}
