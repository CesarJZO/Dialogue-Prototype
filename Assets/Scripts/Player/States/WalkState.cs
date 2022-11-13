using UnityEngine;

namespace Player
{
    public class WalkState : PlayerState
    {
        private float _direction;
        private float _smoothDirection;
        private float _inputVelocity;
        
        public WalkState(PlayerController player) : base(player) { }

        public override void Start()
        {
            _smoothDirection = _direction = player.moveAction.ReadValue<float>();
        }

        public override void HandleInput()
        {
            _direction = player.moveAction.ReadValue<float>();

            if (Mathf.Abs(_smoothDirection) <= 0.05f)
                player.ChangeState(player.idleState);
        }

        public override void FixedUpdate()
        {
            _smoothDirection = Mathf.SmoothDamp(
                _smoothDirection,
                _direction,
                ref _inputVelocity,
                player.smoothTime,
                player.walkSpeed,
                Time.fixedDeltaTime
            );
            player.rigidbody.velocity = player.walkSpeed * _smoothDirection * Vector2.right;
        }

        public override string ToString() => nameof(WalkState);
    }
}
