using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Protag
{
    public class MovementVisuals : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private Transform _bodyTransform;

        [Header("IK")]

        [FormerlySerializedAs("_leftLegTransform")]
        [SerializeField]
        private Transform _leftLegIKTarget;

        [FormerlySerializedAs("_rightLegTransform")]
        [SerializeField]
        private Transform _rightLegIKTarget;

        [Header("Config")]

        [SerializeField]
        private float _lerpFactor;

        [SerializeField]
        private float _footToGroundDistance;

        [SerializeField]
        private float _footRaiseHeight;

        [Header("Events")]

        [SerializeField]
        private UnityEvent _onMove;

        [SerializeField]
        private UnityEvent _onVisualStep;

        private Vector3 _targetLeftLegPosition;
        private Vector3 _targetRightLegPosition;

        private Vector3 _currentLeftLegPosition;
        private Vector3 _currentRightLegPosition;

        private Vector3 _prevCenterPos;

        private void Update()
        {
            float t = 1 - Mathf.Pow(1 - _lerpFactor, Time.deltaTime);

            _currentLeftLegPosition = Vector3.Lerp(
                _currentLeftLegPosition,
                _targetLeftLegPosition,
                t
            );

            _currentRightLegPosition = Vector3.Lerp(
                _currentRightLegPosition,
                _targetRightLegPosition,
                t
            );

            Vector3 bodyPosition = (_currentLeftLegPosition + _currentRightLegPosition) / 2;

            // Sync to transforms
            _bodyTransform.position = bodyPosition;

            if (bodyPosition != _prevCenterPos)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation((bodyPosition - _prevCenterPos).normalized, Vector3.up);
                _bodyTransform.rotation = Quaternion.Lerp(
                    _bodyTransform.rotation,
                    targetRotation,
                    t
                );
            }

            float leftLegDist = Vector3.Distance(
                _currentLeftLegPosition,
                _targetLeftLegPosition
            );

            Vector3 ikLeftTargetPos = _currentLeftLegPosition;
            ikLeftTargetPos.y = Mathf.InverseLerp(0, _footToGroundDistance, leftLegDist) * _footRaiseHeight;

            float rightLegDist = Vector3.Distance(
                _currentRightLegPosition,
                _targetRightLegPosition
            );

            Vector3 ikRightTargetPos = _currentRightLegPosition;
            ikRightTargetPos.y = Mathf.InverseLerp(0, _footToGroundDistance, rightLegDist) * _footRaiseHeight;

            _leftLegIKTarget.position = ikLeftTargetPos;
            _rightLegIKTarget.position = ikRightTargetPos;

            _prevCenterPos = bodyPosition;
        }

        public void StepVisual()
        {
            _onVisualStep?.Invoke();
        }

        public void SetLeftLegPosition(Vector3 position)
        {
            _targetLeftLegPosition = position;
            _onMove?.Invoke();
        }

        public void SetRightLegPosition(Vector3 position)
        {
            _targetRightLegPosition = position;
            _onMove?.Invoke();
        }
    }
}