using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Systems
{
    public class MainGameplay : MonoBehaviour
    {
        [Header("Config")]

        [SerializeField]
        private float gameOverDelay;

        [Header("Events")]

        public UnityEvent OnPlayerStomped;

        public UnityEvent OnMoveToResults;

        private void Start()
        {
            ScoreManager.Instance.SetScore(0);
        }

        public void PlayerStomped()
        {
            OnPlayerStomped?.Invoke();
            MoveToResults().Forget();
        }

        private async UniTaskVoid MoveToResults()
        {
            await UniTask.Delay((int)(gameOverDelay * 1000));
            OnMoveToResults?.Invoke();
        }
    }
}