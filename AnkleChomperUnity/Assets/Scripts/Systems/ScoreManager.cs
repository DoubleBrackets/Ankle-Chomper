using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Systems
{
    public class ScoreManager : MonoBehaviour
    {
        private const string HighScoreKey = "AnkleChomperHighScore";

        [Header("Events")]

        public UnityEvent<int> OnScoreUpdated;

        public static ScoreManager Instance { get; private set; }

        [field: ShowNonSerializedField]
        public int Score { get; private set; }

        [field: ShowNonSerializedField]

        public int HighScore { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            HighScore = GetHighScore();
        }

        public void SetScore(int newScore)
        {
            if (newScore == Score)
            {
                return;
            }

            Score = newScore;
            OnScoreUpdated?.Invoke(Score);
        }

        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public bool TryHighScore()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
                PlayerPrefs.SetInt(HighScoreKey, Score);
                PlayerPrefs.Save();
                return true;
            }

            return false;
        }

        public void IncrementScore()
        {
            SetScore(Score + 1);
        }
    }
}