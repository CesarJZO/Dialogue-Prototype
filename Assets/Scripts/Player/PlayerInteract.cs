using System;
using System.Collections.Generic;
using Interactable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private PlayerInputControls playerInputControls;
        [SerializeField] private float interactRange;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private Transform player;

        private void Start()
        {
            playerInputControls.interactAction.started += InteractActionOnStarted;
        }

        private void InteractActionOnStarted(InputAction.CallbackContext obj)
        {
            var colliders = Array.Empty<Collider2D>();
            Physics2D.OverlapCircleNonAlloc(transform.position, interactRange, colliders, layerMask);
            var closest = GetClosestNpc(colliders);
            
            if (closest.TryGetComponent(out IInteractable npcInteractable))
                npcInteractable.Interact();
        }

        private Collider2D GetClosestNpc(IEnumerable<Collider2D> colliders)
        {
            Collider2D closestNpc = null;
            foreach (var npc in colliders)
            {
                if (closestNpc == null)
                    closestNpc = npc;
                else
                {
                    if (Vector3.Distance(player.position, npc.transform.position) <
                        Vector3.Distance(player.position, closestNpc.transform.position))
                        closestNpc = npc;
                }
            }

            return closestNpc;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }
    }
}
