using Legs;
using NaughtyAttributes;
using Systems;
using UnityEngine;

namespace GameplaySystem
{
    public class StompSpawner : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private StompingLeg _stompingLegPrefab;

        [SerializeField]
        private Transform _protagTransform;

        [Header("Config")]

        [SerializeField]
        private float _baseDelay;

        [SerializeField]
        private float _baseTelegraphDuration;

        [SerializeField]
        private float _inaccuracyDistance;

        [SerializeField]
        private Vector2 _timingRandomMult;

        public bool Spawning { get; set; } = true;

        [ShowNonSerializedField]
        private float _stompTimer;

        private void Start()
        {
            _stompTimer = _baseDelay;
        }

        private void Update()
        {
            if (ScoreManager.Instance.Score > 0 && Spawning)
            {
                _stompTimer -= Time.deltaTime;
            }

            if (_stompTimer <= 0f)
            {
                float telegraphReduction = Mathf.Pow(ScoreManager.Instance.Score, 0.2f);

                SpawnStompingLeg(_baseTelegraphDuration / telegraphReduction * GetRandomTimingMultiplier());
                float delayReduction = Mathf.Sqrt(ScoreManager.Instance.Score);
                _stompTimer = _baseDelay / delayReduction * GetRandomTimingMultiplier();
            }
        }

        private float GetRandomTimingMultiplier()
        {
            return Random.Range(_timingRandomMult.x, _timingRandomMult.y);
        }

        private void SpawnStompingLeg(float telegraphDuration)
        {
            if (_stompingLegPrefab == null || _protagTransform == null)
            {
                Debug.LogWarning("Stomping leg prefab or protag transform is not set.");
                return;
            }

            Vector3 targetPosition = _protagTransform.position;

            targetPosition.y = 0f;

            // Inaccuracy
            Vector2 offset = Random.insideUnitCircle * _inaccuracyDistance;

            targetPosition.x += offset.x;
            targetPosition.z += offset.y;

            StompingLeg newLeg = Instantiate(_stompingLegPrefab,
                targetPosition,
                Quaternion.Euler(0, Random.Range(0f, 360f), 0f));

            newLeg.DoStomp(telegraphDuration);
        }
    }
}