using StatePattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        public float walkSpeed;
        public float smoothTime;
        [SerializeField] private Vector2 interactionSensorSize;

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
            point:interactionSensor.position,
            size: interactionSensorSize,
            angle: 0f,
            layerMask: interactionLayer
        );

        #region State Machine

        private StateMachine _stateMachine;
        public IdleState idleState;
        public WalkState walkState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);

        #endregion

        private void Awake()
        {
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
                new Rect(0, 0, Screen.width, Screen.height),
                $@"CurrentState: {_stateMachine.CurrentState}",
                new GUIStyle
                {
                    fontSize = Screen.height / 20,
                    alignment = TextAnchor.LowerLeft,
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
