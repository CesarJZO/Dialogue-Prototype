using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputControls : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction interactAction;

        [SerializeField] private Actions actions;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            moveAction = _playerInput.actions[actions.move];
            interactAction = _playerInput.actions[actions.interact];
        }
    }

    [Serializable]
    internal struct Actions
    {
        public string move;
        public string interact;
    }
}
