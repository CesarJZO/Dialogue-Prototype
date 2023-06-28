using System;
using System.Collections.Generic;
using CesarJZO;
using CesarJZO.DialogueSystem;
using UnityEngine;

namespace Entities
{
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private Dialogue[] dialogues;

        private Queue<Dialogue> _dialogueQueue;

        private Dialogue _currentDialogue;

        private DialogueManager _dialogueManager;

        private void Awake()
        {
            _dialogueQueue = new Queue<Dialogue>(dialogues);
        }

        private void Start()
        {
            _dialogueManager = DialogueManager.Instance;

            _dialogueManager.ConditionalNodeEvaluated += ConditionalNode;

            try
            {
                _currentDialogue = _dialogueQueue.Dequeue();
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning($"{name} NPC has no dialogues.", this);
            }
        }

        private void ConditionalNode(bool value)
        {
            if (!value) return;

            if (_dialogueQueue.TryDequeue(out Dialogue dialogue))
                _currentDialogue = dialogue;
        }

        public void Interact()
        {
            if (!_currentDialogue) return;

            if (!_dialogueManager) return;

            _dialogueManager.StartDialogue(_currentDialogue);
        }
    }
}
