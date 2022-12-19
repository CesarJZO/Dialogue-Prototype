using StatePattern;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        public float walkSpeed;
        public float smoothTime;

        [Header("Gizmos")]
        [SerializeField] private Vector2 textPos;
        [SerializeField] private int fontSize;

        [HideInInspector] public int horizontalDirection;
        [HideInInspector] public bool interacting;

        #region Dependencies

        [HideInInspector] public new Rigidbody2D rigidbody;
        [FormerlySerializedAs("playerInputManager")] [FormerlySerializedAs("playerInput")] [FormerlySerializedAs("inputManager")] public PlayerInputControls playerInputControls;

        #endregion

        #region State Machine

        private StateMachine _stateMachine;
        public IdleState idleState;
        public WalkState walkState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);
        
        #endregion

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
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
    }
}
