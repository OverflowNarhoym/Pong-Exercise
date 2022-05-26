using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Score : MonoBehaviour
    {
        public TMP_Text leftPlayerScore;
        public TMP_Text rightPlayerScore;

        private void Start()
        {
            UpdatePlayerScores();
        }
        
        public void UpdatePlayerScores()
        {
            leftPlayerScore.text = DataManager.Instance.LeftPlayerScore.ToString();
            rightPlayerScore.text = DataManager.Instance.RightPlayerScore.ToString();
        }
    }
}