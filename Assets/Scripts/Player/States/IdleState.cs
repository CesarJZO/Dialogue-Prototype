using UnityEngine;

namespace Player
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player) { }

        public override void Start()
        {
            player.interacting = false;
            player.rigidbody.velocity = Vector2.zero;
        }

        public override void HandleInput()
        {
            if (player.interacting) return;
            if (player.interactAction.WasPressedThisFrame() && player.CanInteract)
            {
                if (!player.CanInteract.TryGetComponent(out IInteractable interactable)) return;

                player.interacting = true;
                interactable.Interact(() => player.interacting = false);
                return;
            }

            if (player.moveAction.ReadValue<float>() != 0f)
                player.ChangeState(player.walkState);
        }

        public override string ToString() => nameof(IdleState);
    }
}
