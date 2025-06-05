using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScoreScreen
{
    public class ScoreScreenUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        [SerializeField]
        private TMP_Text highScoreText;

        public UnityEvent OnHighScoreAchieved;

        private void Start()
        {
            bool isHighScore = ScoreManager.Instance.TryHighScore();
            scoreText.text = ScoreManager.Instance.Score.ToString();
            highScoreText.text = ScoreManager.Instance.HighScore.ToString();

            if (isHighScore)
            {
                OnHighScoreAchieved?.Invoke();
            }
        }
    }
}