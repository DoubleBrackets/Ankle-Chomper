using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class LiveScoreText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        [SerializeField]
        private UnityEvent onScoreUpdated;

        private void Start()
        {
            UpdateScoreText(ScoreManager.Instance.Score);
            ScoreManager.Instance.OnScoreUpdated.AddListener(UpdateScoreText);
        }

        private void OnDestroy()
        {
            ScoreManager.Instance.OnScoreUpdated.RemoveListener(UpdateScoreText);
        }

        private void UpdateScoreText(int newScore)
        {
            scoreText.text = newScore.ToString();
            onScoreUpdated?.Invoke();
        }
    }
}