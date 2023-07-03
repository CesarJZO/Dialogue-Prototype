using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CesarJZO.DialogueSystem
{
    public class DialogueNpc : MonoBehaviour, IInteractable
    {
        [Header("Conditional")]
        [SerializeField] private bool usedByCondition;
        [Tooltip("If this string is not empty and Used By Condition is true, the condition will be evaluated on start.")]
        [SerializeField] private string flagOnStart;

        [Header("Settings")]
        [SerializeField] private bool dequeueOnEnd;
        [SerializeField] private Dialogue[] dialogues;
        [SerializeField] private Dialogue currentDialogue;

        public UnityEvent<DialogueNode> onNodeChanged;

        private Queue<Dialogue> _dialogueQueue;

        private DialogueManager _dialogueManager;

        private void Awake()
        {
            _dialogueQueue = new Queue<Dialogue>(dialogues);
        }

        private void Start()
        {
            _dialogueManager = DialogueManager.Instance;

            _dialogueManager.ConversationStarted += OnConversationStarted;
            _dialogueManager.NodeChanged += OnNodeChanged;
            _dialogueManager.NodeChanged += node => onNodeChanged?.Invoke(node);
            _dialogueManager.ConversationEnded += OnConversationEnded;

            try
            {
                currentDialogue = _dialogueQueue.Dequeue();
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning($"{name} NPC has no dialogues.", this);
            }
        }

        private void OnConversationEnded()
        {
            if (!dequeueOnEnd) return;

            TryDequeueDialogue();
        }

        private void OnConversationStarted()
        {
            if (!usedByCondition) return;
            if (string.IsNullOrEmpty(flagOnStart) || string.IsNullOrWhiteSpace(flagOnStart)) return;

            if (!TryGetConditionComponent(out DialogueCondition condition)) return;
            if (!condition.EvaluateFlag(flagOnStart)) return;

            if (condition.Action is DialogueCondition.ResponseAction.Dequeue)
                TryDequeueDialogue();
            else
                currentDialogue = condition.DialogueToReplace;
        }

        private void OnNodeChanged(DialogueNode parentNode)
        {
            if (!usedByCondition) return;

            if (!TryGetConditionComponent(out DialogueCondition condition)) return;
            if (!condition.Evaluate(parentNode)) return;

            if (condition.Action is DialogueCondition.ResponseAction.Dequeue)
                TryDequeueDialogue();
            else
                currentDialogue = condition.DialogueToReplace;
        }

        private bool TryGetConditionComponent(out DialogueCondition condition)
        {
            condition = GetComponent<DialogueCondition>();
            return condition;
        }

        private void TryDequeueDialogue()
        {
            if (_dialogueQueue.TryDequeue(out Dialogue dialogue))
                currentDialogue = dialogue;
        }

        public void Interact()
        {
            if (!currentDialogue) return;

            if (!_dialogueManager) return;

            _dialogueManager.StartDialogue(currentDialogue);
        }
    }
}
