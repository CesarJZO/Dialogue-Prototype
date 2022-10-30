namespace Player
{
    public class IdleState : PlayerState
    {
        public bool interacting;
        
        public IdleState(PlayerController player) : base(player) { }

        public override void HandleInput()
        {
            if (interacting) return;

            if (player.CanInteract)
            {
                if (player.interactAction.WasPressedThisFrame())
                    interacting = true;
                return;
            }
            
            if (player.moveAction.ReadValue<float>() != 0f)
                player.ChangeState(player.walkState);
        }

        public override string ToString() => nameof(IdleState);
    }
}
