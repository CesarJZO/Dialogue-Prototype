using System;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public event Action ConversationStarted;
        public event Action ConversationEnded;

        public event Action ConversationUpdated;
        public static DialogueManager Instance { get; private set; }

        public bool HasDialogue => _currentDialogue;

        public Speaker CurrentSpeaker => _currentNode.Speaker;

        public string CurrentText => _currentNode.Text;

        public bool Choosing => _currentNode is ResponseNode;

        public DialogueNode CurrentNode => _currentNode;
        public DialogueNode NextNode
        {
            get
            {
                if (_currentNode.Child is ConditionalNode conditionalNode)
                    return conditionalNode.Child;
                return _currentNode.Child;
            }
        }

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
        ///     Finishes the current conversation and invokes <see cref="ConversationUpdated"/>.
        /// </summary>
        public void Quit()
        {
            _currentDialogue = null;
            _currentNode = null;
            ConversationUpdated?.Invoke();
            ConversationEnded?.Invoke();
        }

        public void Next()
        {
            if (!_currentNode.Child)
            {
                ConversationUpdated?.Invoke();
                ConversationEnded?.Invoke();
                return;
            }

            _currentNode = _currentNode.Child;

            ConversationUpdated?.Invoke();
        }
    }
}
