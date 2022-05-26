using Ball;
using Data;
using Player;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public enum Difficulty { Easy, Medium, Hard }
        
        public GameObject mainMenuButton;
        public GameObject scoreText;
        public GameObject difficultyMenu;

        public GameObject players;
        public GameObject ball;
        
        public void StartSoloGame()
        {
            players.GetComponent<PlayerController>().UseAI = true;
            ChooseDifficulty();
        }

        public void StartMultiGame()
        {
            players.GetComponent<PlayerController>().UseAI = false;
            gameObject.SetActive(false);
            StartGame();
        }

        private void ChooseDifficulty()
        {
            difficultyMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        public void SetEasy()
        {
            players.GetComponent<PlayerController>().ChooseDifficulty(Difficulty.Easy);
            difficultyMenu.SetActive(false);
            StartGame();
        }
        
        public void SetMedium()
        {
            players.GetComponent<PlayerController>().ChooseDifficulty(Difficulty.Medium);
            difficultyMenu.SetActive(false);
            StartGame();
        }
        
        public void SetHard()
        {
            players.GetComponent<PlayerController>().ChooseDifficulty(Difficulty.Hard);
            difficultyMenu.SetActive(false);
            StartGame();
        }

        private void StartGame()
        {
            mainMenuButton.SetActive(true);
            scoreText.SetActive(true);
            DataManager.FetchScore();
            players.GetComponent<PlayerController>().Pause(false);
            ball.GetComponent<BallMovement>().Pause(false);
        }

        public void Quit()
        {
            Application.OpenURL("about:blank");
        }
    }
}
