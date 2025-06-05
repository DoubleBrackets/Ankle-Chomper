using System.Threading;
using Cysharp.Threading.Tasks;
using Protag;
using UnityEngine;
using UnityEngine.Events;

namespace Legs
{
    public class StompingLeg : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private Transform _stompCenter;

        [SerializeField]
        private Transform _footBody;

        [Header("Config")]

        [SerializeField]
        private float _stompRadius;

        [SerializeField]
        private LayerMask _kidLayerMask;

        [SerializeField]
        private float _liftSpeed;

        [SerializeField]
        private float _holdDuration;

        [SerializeField]
        private float _liftDuration;

        [Header("Events")]

        public UnityEvent OnStompEffects;

        public UnityEvent OnShowTelegraph;
        public UnityEvent OnHideTelegraph;

        private void OnDrawGizmos()
        {
            if (_stompCenter == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_stompCenter.position, _stompRadius);
        }

        public void DoStompEffects()
        {
            OnStompEffects?.Invoke();
        }

        public void DoStomp(float telegraphDuration)
        {
            Debug.Log("STOMP", gameObject);
            DoStompAsync(telegraphDuration, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid DoStompAsync(float telegraphDuration, CancellationToken token)
        {
            OnShowTelegraph?.Invoke();

            await UniTask.Delay((int)(telegraphDuration * 1000));

            token.ThrowIfCancellationRequested();

            OnHideTelegraph?.Invoke();

            DoStompCheck();
            DoStompEffects();

            await UniTask.Delay((int)(_holdDuration * 1000));

            token.ThrowIfCancellationRequested();

            // Move up
            var elapsedTime = 0f;

            while (elapsedTime < _liftDuration)
            {
                _footBody.position += Vector3.up * _liftSpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;

                await UniTask.Yield();

                token.ThrowIfCancellationRequested();
            }

            // Destroy
            Destroy(gameObject);
        }

        private void DoStompCheck()
        {
            Collider[] hits = Physics.OverlapSphere(_stompCenter.position, _stompRadius, _kidLayerMask);

            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.TryGetComponent(out StompTarget stompTarget))
                    {
                        stompTarget.Stomp();
                    }
                }
            }
            else
            {
                Debug.Log("No legs stomped.");
            }
        }
    }
}