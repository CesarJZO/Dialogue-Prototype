using CesarJZO.StatePattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CesarJZO.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        public float walkSpeed;
        public float smoothTime;

        [Header("Physics")]
        [HideInInspector] public int horizontalDirection;

        #region Dependencies

        [HideInInspector] public new Rigidbody2D rigidbody;
        [SerializeField] private PlayerInput playerInput;
        [HideInInspector] public InputAction moveAction;

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

            moveAction = playerInput.actions["Move"];

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
            var currentState = _stateMachine.CurrentState as PlayerState;
            currentState!.HandleInput();
            currentState.Update();
        }

        private void FixedUpdate() => _stateMachine.CurrentState.FixedUpdate();

        private void LateUpdate() => _stateMachine.CurrentState.LateUpdate();

        private void OnGUI()
        {
            GUI.Label(
                new Rect(0f, 0f, Screen.width, Screen.height),
                _stateMachine.CurrentState.ToString(),
                new GUIStyle
                {
                    fontSize = 16,
                    alignment = TextAnchor.UpperLeft,
                    normal = { textColor = Color.white }
                }
            );
        }
    }
}
