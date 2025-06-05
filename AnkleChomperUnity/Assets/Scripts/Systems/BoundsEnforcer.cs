using Legs;
using UnityEngine;
using UnityEngine.Events;

namespace Systems
{
    public class BoundsEnforcer : MonoBehaviour
    {
        [SerializeField]
        private Leg _legPrefab;

        [SerializeField]
        private Transform _playerTransform;

        [SerializeField]
        private float _boundsRadius;

        [SerializeField]
        private UnityEvent _onPlayerOutOfBounds;

        private bool stomped;

        private void Update()
        {
            if (_playerTransform == null || _legPrefab == null || stomped)
            {
                return;
            }

            Vector3 playerPosition = _playerTransform.position;
            playerPosition.y = 0f;

            float distanceFromCenter = Vector3.Distance(playerPosition, transform.position);

            if (distanceFromCenter > _boundsRadius)
            {
                _onPlayerOutOfBounds?.Invoke();

                Instantiate(_legPrefab, playerPosition, Quaternion.Euler(0f, Random.Range(0, 360f), 0f), transform);
                _legPrefab.DoStompEffects();

                stomped = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _boundsRadius);
        }
    }
}