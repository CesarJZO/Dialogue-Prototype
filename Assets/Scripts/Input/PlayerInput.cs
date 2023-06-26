using System;
using GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CesarJZO.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance { get; private set; }

        #region Ground events

        public event Action InteractPerformed;

        #endregion

        #region UI events

        public event Action NextPerformed;

        #endregion

        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            Instance = this;
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            var gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.StateUpdated += OnGameStateUpdated;
            }

            _playerInputActions.Ground.Interact.performed += OnInteractPerformed;

            _playerInputActions.UI.Next.performed += OnNextPerformed;

            _playerInputActions.Ground.Enable();
        }

        private void OnGameStateUpdated(GameManager.State state)
        {
            _playerInputActions.Ground.Disable();
            _playerInputActions.UI.Disable();

            switch (state)
            {
                case GameManager.State.Playing:
                    _playerInputActions.Ground.Enable();
                    break;
                case GameManager.State.Dialogue:
                    _playerInputActions.UI.Enable();
                    break;
            }
        }

        private void OnDisable()
        {
            _playerInputActions.Ground.Interact.performed -= OnInteractPerformed;

            _playerInputActions.UI.Next.performed -= OnNextPerformed;

            _playerInputActions.Ground.Disable();
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            InteractPerformed?.Invoke();
        }

        private void OnNextPerformed(InputAction.CallbackContext context)
        {
            NextPerformed?.Invoke();
        }
    }
}
