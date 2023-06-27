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
                gameManager.StateUpdated += OnGameStateUpdated;

            EnableGround();
        }

        private void OnGameStateUpdated(GameManager.State state)
        {
            DisableAll();

            switch (state)
            {
                case GameManager.State.Playing:
                    EnableGround();
                    break;
                case GameManager.State.Dialogue:
                    EnableUI();
                    break;
            }
        }

        private void OnDisable()
        {
            var gameManager = GameManager.Instance;

            if (gameManager)
                gameManager.StateUpdated -= OnGameStateUpdated;

            DisableGround();
            DisableUI();
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            InteractPerformed?.Invoke();
        }

        private void OnNextPerformed(InputAction.CallbackContext context)
        {
            NextPerformed?.Invoke();
        }

        private void EnableGround()
        {
            _playerInputActions.Ground.Interact.performed += OnInteractPerformed;
            _playerInputActions.Ground.Enable();
        }

        private void EnableUI()
        {
            _playerInputActions.UI.Next.performed += OnNextPerformed;
            _playerInputActions.UI.Enable();
        }

        private void DisableGround()
        {
            _playerInputActions.Ground.Interact.performed -= OnInteractPerformed;
            _playerInputActions.Ground.Disable();
        }

        private void DisableUI()
        {
            _playerInputActions.UI.Next.performed -= OnNextPerformed;
            _playerInputActions.UI.Disable();
        }

        private void DisableAll()
        {
            DisableGround();
            DisableUI();
        }
    }
}
