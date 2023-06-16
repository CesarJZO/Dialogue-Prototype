using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CesarJZO.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance { get; private set; }

        public event Action InteractPerformed;

        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            Instance = this;
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _playerInputActions.Ground.Enable();
            _playerInputActions.Ground.Interact.performed += OnInteractPerformed;
        }

        private void OnDisable()
        {
            _playerInputActions.Ground.Disable();
            _playerInputActions.Ground.Interact.performed -= OnInteractPerformed;
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            InteractPerformed?.Invoke();
        }
    }
}
