using System;
using StatePattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [Header("Settings")]
        public float walkSpeed;
        public float smoothTime;
        [SerializeField] private Vector2 interactionSensorSize;

        [Header("Gizmos")]
        [SerializeField] private Vector2 textPos;
        [SerializeField] private int fontSize;
        
        [Header("Physics")]
        public LayerMask interactionLayer;
        [SerializeField] private Transform interactionSensor;

        [HideInInspector] public int horizontalDirection;
        [HideInInspector] public bool interacting;

        #region Dependencies

        [HideInInspector] public new Rigidbody2D rigidbody;
        [SerializeField] private PlayerInput playerInput;
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction interactAction;

        #endregion

        public Collider2D CanInteract => Physics2D.OverlapBox(
            interactionSensor.position,
            interactionSensorSize,
            0f, 
            interactionLayer 
        );

        #region State Machine

        private StateMachine _stateMachine;
        public IdleState idleState;
        public WalkState walkState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);
        
        #endregion

        private void Awake()
        {
            Instance = this;
            rigidbody = GetComponent<Rigidbody2D>();
            
            moveAction = playerInput.actions["Move"];
            interactAction = playerInput.actions["Interact"];

            idleState = new IdleState(this);
            walkState = new WalkState(this);
            _stateMachine = new StateMachine(idleState);
        }

        private void Start()
        {
            horizontalDirection = 1;
        }

        private void Update()
        {
            var currentState = (PlayerState)_stateMachine.CurrentState;

            currentState.HandleInput();
            currentState.Update();
        }

        
        private void FixedUpdate() => _stateMachine.CurrentState.FixedUpdate();
        
        private void LateUpdate() => _stateMachine.CurrentState.LateUpdate();

        private void OnGUI()
        {
            GUI.Label(
                new Rect(textPos, Vector2.one),
                _stateMachine.CurrentState.ToString(),
                new GUIStyle
                {
                    fontSize = fontSize, 
                    normal = { textColor = Color.white }
                }
            );
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Application.isPlaying && CanInteract ? Color.green : Color.blue;

            Gizmos.DrawWireCube(interactionSensor.position, interactionSensorSize);
        }
    }
}
