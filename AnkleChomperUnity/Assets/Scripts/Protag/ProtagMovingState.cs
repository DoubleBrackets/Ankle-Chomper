using UnityEngine;
using UnityEngine.Events;

namespace Protag
{
    public class ProtagMovingState : ProtagState
    {
        [Header("Depends")]

        [SerializeField]
        private Transform _bodyTransform;

        [SerializeField]
        private Rigidbody _bodyRigidbody;

        [Header("Config")]

        [SerializeField]
        private float _strideAngle;

        /// <summary>
        ///     Gap between the center of the body and each leg.
        ///     Identical to the radius of the circle "traced" by the center point
        ///     on each stride
        /// </summary>
        [SerializeField]
        private float _legGapFromCenter;

        [Header("Events")]

        [SerializeField]
        private UnityEvent _onStride;

        [SerializeField]
        private UnityEvent<Vector3> _onLeftFootPositionChanged;

        [SerializeField]
        private UnityEvent<Vector3> _onRightFootPositionChanged;

        [SerializeField]
        private UnityEvent<Vector3> _onCenterPositionChanged;

        public bool MovementEnabled { get; set; }

        private Vector3 _leftFootPosition;
        private Vector3 _rightFootPosition;

        private Vector3 _centerPosition;
        private Vector3 _prevCenterPos;

        /// <summary>
        ///     First stride is half the stride angle to get a "crossing over" effect
        /// </summary>
        private bool _isFirstStride = true;

        private void Start()
        {
            _prevCenterPos = Vector3.down;
            ResolveNewPosition(
                _bodyTransform.position,
                _bodyTransform.forward
            );
        }

        private void FixedUpdate()
        {
            _bodyRigidbody.linearVelocity = Vector3.zero;

            _centerPosition = _bodyRigidbody.position;

            _bodyTransform.position = _centerPosition;
            _bodyTransform.rotation = _bodyRigidbody.rotation;

            // Move Legs
            _leftFootPosition = _centerPosition - _bodyTransform.right * _legGapFromCenter;
            _rightFootPosition = _centerPosition + _bodyTransform.right * _legGapFromCenter;

            // Update positions
            if (_centerPosition != _prevCenterPos)
            {
                _onLeftFootPositionChanged.Invoke(_leftFootPosition);
                _onRightFootPositionChanged.Invoke(_rightFootPosition);
                _onCenterPositionChanged.Invoke(_centerPosition);
            }

            _prevCenterPos = _centerPosition;
        }

        private void OnDrawGizmos()
        {
            // Editor gizmos
            if (_bodyTransform == null)
            {
                return;
            }

            Vector3 centerPos = _bodyTransform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(centerPos, centerPos + _bodyTransform.forward * _legGapFromCenter);

            Vector3 rightLegPos = centerPos + _bodyTransform.right * _legGapFromCenter;
            Vector3 leftLegPos = centerPos - _bodyTransform.right * _legGapFromCenter;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(rightLegPos, _legGapFromCenter);
            Gizmos.DrawLine(centerPos, rightLegPos);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(leftLegPos, _legGapFromCenter);
            Gizmos.DrawLine(centerPos, leftLegPos);
        }

        public void StrideLeg(bool strideRightLeg)
        {
            if (_bodyTransform == null || !MovementEnabled)
            {
                return;
            }

            Vector3 centerPose = _centerPosition;
            Vector3 pivotLegPos = strideRightLeg ? _leftFootPosition : _rightFootPosition;
            Vector3 forward = _bodyTransform.forward;

            float usedStrideAngle = _isFirstStride ? _strideAngle / 2 : _strideAngle;

            _isFirstStride = false;

            StrideLegForward(centerPose,
                pivotLegPos,
                forward,
                usedStrideAngle,
                out Vector3 newCenterPosition,
                out Vector3 newForward);

            ResolveNewPosition(newCenterPosition, newForward);

            _onStride.Invoke();
        }

        public void StrideRightLeg()
        {
            StrideLeg(true);
        }

        public void StrideLeftLeg()
        {
            StrideLeg(false);
        }

        private void ResolveNewPosition(Vector3 newCenterPosition, Vector3 newForward)
        {
            _bodyRigidbody.Move(
                newCenterPosition,
                Quaternion.LookRotation(newForward, Vector3.up));
        }

        private void StrideLegForward(
            Vector3 centerPose,
            Vector3 pivotLegPos,
            Vector3 forward,
            float strideDegrees,
            out Vector3 newCenterPos,
            out Vector3 newForward)
        {
            /*
             * Striding a leg forward is equivalent to "pivoting" around the other leg some degrees.
             *
             */
            Vector3 pivotToCenter = centerPose - pivotLegPos;
            Vector3 rotationAxis = Vector3.Cross(pivotToCenter, forward).normalized;

            // Rotate the center position around the pivot leg position
            newCenterPos = Quaternion.AngleAxis(strideDegrees, rotationAxis) * pivotToCenter + pivotLegPos;

            // Rotate forward
            newForward = Quaternion.AngleAxis(strideDegrees, rotationAxis) * forward;
        }

        public override void EnterState()
        {
            MovementEnabled = true;
        }

        public override void OnExitState()
        {
            MovementEnabled = false;
        }

        public override bool CanEnterState()
        {
            return true;
        }
    }
}