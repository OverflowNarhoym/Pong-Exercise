using System.Collections;
using Data;
using Pausable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ball
{
    public class BallMovement : MonoBehaviour, IPausable
    {
        private Rigidbody2D _rigidbody2D;
        private Vector2 _velocityBeforePause;

        private float _speed = 410.0f;
        private float _speedMultiplier = 1.07f;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            IsPaused = true;
            _velocityBeforePause = GetNewDirection();
        }

        private Vector2 GetNewDirection()
        {
            Vector2 vector = Vector2.zero;
            vector.x = 0.6f;
            vector.y = Random.Range(0.1f, 1.0f);

            if (Random.Range(0, 2) == 1) vector.y *= -1;
            if (Random.Range(0, 2) == 1) vector.x *= -1;
            return vector.normalized * (_speed * Time.fixedDeltaTime);
        }
        
        private void ResetBall()
        {
            transform.position = Vector3.zero;
            ResetSpeed();
            if (IsPaused) _velocityBeforePause = GetNewDirection();
            else _rigidbody2D.velocity = GetNewDirection();
        }

        public void ResetSpeed()
        {
            _speed = 410;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
                Bounce(false);
            else if (col.gameObject.CompareTag("Limit"))
                Bounce(true);
            else if (col.gameObject.CompareTag("DeathLimit"))
            {
                if (transform.position.x < 0) DataManager.Instance.UpdateRightPlayerScore();
                else DataManager.Instance.UpdateLeftPlayerScore();
                StartCoroutine(WaitForNewBall());
            }
        }

        private void Bounce(bool up)
        {
            if (IsPaused) return;
            Vector2 velocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity = up ? new Vector2(velocity.x, -velocity.y) : new Vector2(-velocity.x, velocity.y) * _speedMultiplier;
        }

        public bool IsPaused { get; set; }
        public void Pause(bool pause)
        {
            IsPaused = pause;
            switch (pause)
            {
                case true:
                    _velocityBeforePause = _rigidbody2D.velocity;
                    _rigidbody2D.velocity = Vector2.zero;
                    break;
                case false:
                    _rigidbody2D.velocity = _velocityBeforePause;
                    break;
            }
        }

        private IEnumerator WaitForNewBall()
        {
            yield return new WaitForSeconds(0.5f);
            ResetBall();
        }
    }
}