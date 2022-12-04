using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JSON
{
    public class JsonTest : MonoBehaviour
    {
        public PlayerInput playerInput;
        public InputAction action;
        private void Start()
        {
            action = playerInput.actions["Interact"];
            action.performed += OnRead;
        }

        private static void OnRead(InputAction.CallbackContext callbackContext)
        {
            var json = File.ReadAllText(Application.dataPath + "/Scripts/JSON/player_info.json");
            Debug.Log(PlayerInfo.CreateFromJson(json).ToString());
        }
    }
}