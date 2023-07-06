using UnityEngine;

namespace CesarJZO.Player
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player) { }

        public override void Start()
        {
            player.rigidbody.velocity = Vector2.zero;
        }

        public override void HandleInput()
        {
            if (player.moveAction.ReadValue<float>() != 0f)
                player.ChangeState(player.walkState);
        }

        public override string ToString() => nameof(IdleState);
    }
}
