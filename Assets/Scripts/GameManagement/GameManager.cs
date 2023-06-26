using System;
using CesarJZO.DialogueSystem;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action<State> StateUpdated;

        public enum State
        {
            Playing,
            Paused,
            Dialogue
        }

        [SerializeField] private State currentState;

        public State CurrentState => currentState;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var dialogueManager = DialogueManager.Instance;
            dialogueManager.ConversationStarted += () => UpdateState(State.Dialogue);
            dialogueManager.ConversationEnded += () => UpdateState(State.Playing);
        }

        private void UpdateState(State state)
        {
            currentState = state;
            StateUpdated?.Invoke(state);
        }
    }
}
