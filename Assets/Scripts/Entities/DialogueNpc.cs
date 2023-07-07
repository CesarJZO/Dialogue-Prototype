using System;
using System.Collections.Generic;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class DialogueNpc : MonoBehaviour, IInteractable
    {
        [SerializeField] private Dialogue[] dialogues;
        [SerializeField] private Dialogue currentDialogue;

        private Queue<Dialogue> _dialogueQueue;

        private DialogueManager _dialogueManager;

        private void Awake()
        {
            _dialogueQueue = new Queue<Dialogue>(dialogues);
        }

        private void Start()
        {
            _dialogueManager = DialogueManager.Instance;

            _dialogueManager.ConditionalNodeEvaluated += OnConditionalNodeEvaluated;

            try
            {
                currentDialogue = _dialogueQueue.Dequeue();
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning($"{name} NPC has no dialogues.", this);
            }
        }

        private void OnConditionalNodeEvaluated(bool hadItem)
        {
            var onItemMatch = GetComponent<DialogueOnItemMatch>();

            if (onItemMatch)
                onItemMatch.OnItemMatch(hadItem);
        }

        public void Interact()
        {
            if (!currentDialogue) return;

            if (!_dialogueManager) return;

            _dialogueManager.StartDialogue(currentDialogue);
        }

        public void DequeueDialogue()
        {
            if (_dialogueQueue.TryDequeue(out Dialogue dialogue))
                currentDialogue = dialogue;
        }
    }
}
