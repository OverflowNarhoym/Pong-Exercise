using Ball;
using UI;
using UnityEngine;

namespace Data
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;

        private static Score _scoreUI;
        private static GameObject _playerLeft;
        private static GameObject _playerRight;
        private static GameObject _ball;

        public int LeftPlayerScore { get; private set; }
        public int RightPlayerScore { get; private set; }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public static void FetchScore()
        {
            _scoreUI = GameObject.Find("ScoreText").GetComponent<Score>();
            _playerLeft = GameObject.Find("PlayerLeft");
            _playerRight = GameObject.Find("PlayerRight");
            _ball = GameObject.Find("Ball");
        }

        public void UpdateLeftPlayerScore()
        {
            ++LeftPlayerScore;
            _scoreUI.UpdatePlayerScores();
        }

        public void UpdateRightPlayerScore()
        {
            ++RightPlayerScore;
            _scoreUI.UpdatePlayerScores();
        }

        public void ResetGame()
        {
            ResetScores();
            _ball.GetComponent<BallMovement>().ResetSpeed();
            _playerLeft.transform.SetPositionAndRotation(new Vector3(-9, 0, 0), _playerLeft.transform.rotation);
            _playerRight.transform.SetPositionAndRotation(new Vector3(9, 0, 0), _playerRight.transform.rotation);
            _ball.transform.SetPositionAndRotation(new Vector3(0, 0, 0), _ball.transform.rotation);
        }

        private void ResetScores()
        {
            LeftPlayerScore = 0;
            RightPlayerScore = 0;
            _scoreUI.UpdatePlayerScores();
        }
    }
}
