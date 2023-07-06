using System;
using CesarJZO.Player;
using UnityEngine;

namespace CesarJZO.UI
{
    public class PlayerInteractionUI : MonoBehaviour
    {
        [SerializeField] private PlayerInteractionHandler playerInteractionHandler;
        [SerializeField] private Vector2 offset;

        private void Start()
        {
            playerInteractionHandler.InteractableChanged += OnInteractableChanged;

            gameObject.SetActive(false);
        }

        private void OnInteractableChanged(Transform interactable)
        {
            gameObject.SetActive(interactable);
            if (!interactable) return;
            transform.position = interactable.position + (Vector3) offset;
        }
    }
}
