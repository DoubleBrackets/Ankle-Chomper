using Cysharp.Threading.Tasks;
using Legs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace Protag
{
    public class ProtagChomping : ProtagState
    {
        [Header("Depends")]

        [SerializeField]
        private Transform _sweepCenter;

        [SerializeField]
        private Transform _visualHeadTransform;

        [SerializeField]
        private Rigidbody _rb;

        [Header("Config")]

        [SerializeField]
        private LayerMask _legLayerMask;

        [SerializeField]
        private float _sweepRadius;

        [Header("Head IK")]

        [SerializeField]
        private Transform _headIKTarget;

        [SerializeField]
        private MultiAimConstraint _headIKConstraint;

        [SerializeField]
        private float _targetVerticalOffset;

        [Header("Events")]

        [SerializeField]
        public UnityEvent OnChomped;

        public bool ChompingEnabled { get; set; }

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

            if (_targetedLeg != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(
                    _visualHeadTransform.position,
                    _targetedLeg.GetChompTargetPosition()
                );
            }
        }

        private void RotateHead()
        {
            float t = 1 - Mathf.Pow(1 - 0.999f, Time.deltaTime);

            _headIKConstraint.weight = Mathf.Lerp(_headIKConstraint.weight, _targetedLeg != null ? 1.0f : 0.0f, t);

            Animator.SetBool("Aiming", _targetedLeg != null);

            // Rotate head towards the targeted leg
            if (_targetedLeg != null)
            {
                Vector3 targetPos = _targetedLeg.GetChompTargetPosition();
                _headIKTarget.position = targetPos + Vector3.up * _targetVerticalOffset;
            }
        }

        public void Chomp()
        {
            if (!ChompingEnabled || _targetedLeg == null)
            {
                return;
            }

            Vector3 targetPos = _targetedLeg.GetChompTargetPosition();
            Vector3 forward = (targetPos - _rb.position).normalized;

            _targetedLeg.Eaten();
            _targetedLeg.SetTargeted(false);
            _targetedLeg = null;
            OnChomped?.Invoke();

            targetPos.y = _rb.position.y;
            _rb.Move(targetPos, Quaternion.LookRotation(forward, Vector3.up));
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

        public override void EnterState()
        {
            Animator.Play("kid_armature|Chomping", 0, 0.0f);
            ChompTaskAsync().Forget();
        }

        private async UniTaskVoid ChompTaskAsync()
        {
            ChompingEnabled = true;
            Chomp();
            await UniTask.WaitForSeconds(0.5f);
            _controller.SwitchToDefaultState();
        }

        public override void OnExitState()
        {
            ChompingEnabled = false;
        }

        public override bool CanEnterState()
        {
            return _targetedLeg != null;
        }
    }
}