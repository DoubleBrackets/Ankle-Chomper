using UnityEngine;
using UnityEngine.Events;

namespace Protag
{
    public class MovementVisuals : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private Transform _bodyTransform;

        [SerializeField]
        private Transform _leftLegTransform;

        [SerializeField]
        private Transform _rightLegTransform;

        [Header("Config")]

        [SerializeField]
        private float _lerpFactor;

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
                _bodyTransform.forward = (bodyPosition - _prevCenterPos).normalized;
            }

            _leftLegTransform.position = _currentLeftLegPosition;
            _rightLegTransform.position = _currentRightLegPosition;

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