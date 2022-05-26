using System;
using Pausable;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour, PlayerControl.IPlayerControllerActions, IPausable
    {
        private PlayerControl _controls;

        private const float EasyOffset = 4.5f;
        private const float MediumOffset = 2.0f;
        private const float HardOffset = 0.5f;

        private const float Speed  = 420.0f;

        private Rigidbody2D _leftPlayerRb2D;
        private Rigidbody2D _rightPlayerRb2D;
        
        private float _moveAxisLeft;
        private float _moveAxisRight;
        private float _previousAgentDecision;
        private float _agentDifficultyOffset;

        private GameObject _ballPosition;
        
        public bool UseAI { get; set; }

        private void Awake()
        {
            _leftPlayerRb2D = GameObject.Find("PlayerLeft").GetComponent<Rigidbody2D>();
            _rightPlayerRb2D = GameObject.Find("PlayerRight").GetComponent<Rigidbody2D>();
            _ballPosition = GameObject.Find("Ball");
        }

        private void Start()
        {
            IsPaused = true;
        }

        public void ChooseDifficulty(MainMenu.Difficulty difficulty)
        {
            switch (difficulty)
            {
                case MainMenu.Difficulty.Easy:
                    _agentDifficultyOffset = EasyOffset;
                    break;
                case MainMenu.Difficulty.Medium:
                    _agentDifficultyOffset = MediumOffset;
                    break;
                case MainMenu.Difficulty.Hard:
                    _agentDifficultyOffset = HardOffset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
        
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new PlayerControl();
                _controls.PlayerController.SetCallbacks(this);
            }
            _controls.PlayerController.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void FixedUpdate()
        {
            if (IsPaused) return;
            
            Vector2 newPosRight = Vector2.up * (_moveAxisRight * Speed * Time.fixedDeltaTime);
            _rightPlayerRb2D.velocity = newPosRight;
            
            Vector2 newPosLeft;
            if (!UseAI) newPosLeft = Vector2.up * (_moveAxisLeft * Speed * Time.fixedDeltaTime);
            else newPosLeft = Vector2.up * (AgentDecision() * Speed * Time.fixedDeltaTime);
            _leftPlayerRb2D.velocity = newPosLeft;
        }

        public void OnMoveLeftPlayer(InputAction.CallbackContext context)
        {
            if (UseAI) return;
            if (this.name.Equals("PlayerRight")) return;
            _moveAxisLeft = context.ReadValue<float>();
        }

        public void OnMoveRightPlayer(InputAction.CallbackContext context)
        {
            if (this.name.Equals("PlayerLeft")) return;
            _moveAxisRight = context.ReadValue<float>();
        }

        private float AgentDecision()
        {
            if (IsPaused) return 0.0f;
            
            float ballPos = _ballPosition.transform.position.y;
            float curPos = _leftPlayerRb2D.transform.position.y;
            
            if (ballPos - curPos > _agentDifficultyOffset) _previousAgentDecision = 1.0f;
            else if (ballPos - curPos < -_agentDifficultyOffset) _previousAgentDecision = -1.0f;
            return _previousAgentDecision;
        }

        public bool IsPaused { get; set; }
        public void Pause(bool pause)
        {
            IsPaused = pause;
            if (!pause) return;
            _leftPlayerRb2D.velocity = Vector2.zero;
            _rightPlayerRb2D.velocity = Vector2.zero;
        }
    }
}