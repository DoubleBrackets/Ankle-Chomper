using Legs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Protag
{
    public class ProtagChomping : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _legLayerMask;

        [SerializeField]
        private float _sweepRadius;

        [SerializeField]
        private Transform _sweepCenter;

        [SerializeField]
        private Transform _visualHeadTransform;

        [SerializeField]
        private Rigidbody _rb;

        [Header("Events")]

        [SerializeField]
        public UnityEvent OnChomped;

        [ShowNonSerializedField]
        private Leg _targetedLeg;

        private void Update()
        {
            RotateHead();
        }

        private void FixedUpdate()
        {
            Leg closestLeg = SweepForClosestLeg();

            if (closestLeg == null)
            {
                if (_targetedLeg != null)
                {
                    _targetedLeg.SetTargeted(false);
                }

                _targetedLeg = null;
            }
            else
            {
                if (_targetedLeg != null && _targetedLeg != closestLeg)
                {
                    _targetedLeg.SetTargeted(false);
                }

                closestLeg.SetTargeted(true);
            }

            _targetedLeg = closestLeg;
        }

        private void OnDrawGizmos()
        {
            if (_sweepCenter == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_sweepCenter.position, _sweepRadius);
        }

        private void RotateHead()
        {
            float t = 1 - Mathf.Pow(1 - 0.999f, Time.deltaTime);

            // Rotate head towards the targeted leg
            if (_targetedLeg != null)
            {
                Vector3 directionToLeg = _targetedLeg.GetChompTargetPosition() - _visualHeadTransform.position;
                if (directionToLeg.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToLeg, Vector3.up);
                    _visualHeadTransform.rotation = Quaternion.Lerp(
                        _visualHeadTransform.rotation,
                        targetRotation,
                        t
                    );
                }
            }
            else
            {
                _visualHeadTransform.localRotation = Quaternion.Lerp(
                    _visualHeadTransform.localRotation,
                    Quaternion.identity,
                    t
                );
            }
        }

        public void Chomp()
        {
            if (_targetedLeg != null)
            {
                Vector3 targetPos = _targetedLeg.GetChompTargetPosition();
                Vector3 forward = (targetPos - _rb.position).normalized;

                _targetedLeg.Eaten();
                _targetedLeg.SetTargeted(false);
                _targetedLeg = null;
                OnChomped?.Invoke();

                targetPos.y = _rb.position.y;
                _rb.Move(targetPos, Quaternion.LookRotation(forward, Vector3.up));
            }
        }

        private Leg SweepForClosestLeg()
        {
            Collider[] colliders = Physics.OverlapSphere(_sweepCenter.position, _sweepRadius, _legLayerMask);

            if (colliders.Length == 0)
            {
                return null;
            }

            Leg closestLeg = null;
            var closestDistance = float.MaxValue;

            foreach (Collider coll in colliders)
            {
                var leg = coll.GetComponentInParent<Leg>();
                if (leg == null || !leg.IsValidTarget())
                {
                    continue;
                }

                float distance = Vector3.Distance(_sweepCenter.position, leg.GetChompTargetPosition());
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLeg = leg;
                }
            }

            return closestLeg;
        }
    }
}