using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public event Action ConversationUpdated;
        public static DialogueManager Instance { get; private set; }

        public Speaker CurrentSpeaker => _currentNode.Speaker;
        public string CurrentText => _currentNode.Text;

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
        }

        public Type GetNextType()
        {
            DialogueNode child = _currentNode.Child;

            if (child)
            {
                return child switch
                {
                    ResponseNode => typeof(ResponseNode),
                    ConditionalNode => typeof(ConditionalNode),
                    _ => typeof(SimpleNode)
                };
            }

            return null;
        }

        public void Next()
        {
            if (!_currentNode.Child) return;

            _currentNode = _currentNode.Child is SimpleNode
                ? _currentNode.Child
                : _currentNode.Child.Child;

            ConversationUpdated?.Invoke();
        }

        /// <summary>
        ///     If the current node is a response node, returns the responses.
        /// </summary>
        /// <param name="responses">Set the responses if the current node is a response node.</param>
        /// <returns>Whether the current node is a response node and if it was possible to set the responses.</returns>
        public bool TryGetResponses(out IEnumerable<Response> responses)
        {
            if (_currentNode is ResponseNode responseNode)
            {
                responses = responseNode.Responses;
                return true;
            }

            responses = Enumerable.Empty<Response>();
            return false;
        }
    }
}
