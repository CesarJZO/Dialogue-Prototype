using StatePattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        public float walkSpeed;
        public float smoothTime;
        [SerializeField] private Vector2 interactionSensorSize;

        [Header("Physics")]
        public LayerMask interactionLayer;
        [SerializeField] private Transform interactionSensor;

        [HideInInspector] public new Rigidbody2D rigidbody;

        private PlayerInput _playerInput;
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction interactAction;

        [HideInInspector] public short horizontalDirection;

        public bool CanInteract => Physics2D.OverlapBox(
            interactionSensor.position,
            interactionSensorSize,
            interactionSensor.rotation.z, 
            interactionLayer 
        );

        #region State Machine

        private StateMachine _stateMachine;
        public IdleState idleState;
        public WalkState walkState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);
        
        #endregion
        
        public bool Interacting
        {
            set => idleState.interacting = value;
            get => idleState.interacting;
        }
        
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            _playerInput = GetComponent<PlayerInput>();
            moveAction = _playerInput.actions["Move"];
            interactAction = _playerInput.actions["Interact"];

            idleState = new IdleState(this);
            walkState = new WalkState(this);
            _stateMachine = new StateMachine(idleState);
        }

        private void Update()
        {
            var currentState = (PlayerState)_stateMachine.CurrentState;
            currentState.HandleInput();
            currentState.Update();
        }

        
        private void FixedUpdate() => _stateMachine.CurrentState.FixedUpdate();
        
        private void LateUpdate() => _stateMachine.CurrentState.LateUpdate();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(interactionSensor.position, interactionSensorSize);
        }
    }
}
