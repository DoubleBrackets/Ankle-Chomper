using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    public class InputMono : MonoBehaviour
    {
        [Header("Events")]

        [SerializeField]
        public UnityEvent OnLeftStridePressed;

        [SerializeField]
        public UnityEvent OnRightStridePressed;

        [SerializeField]
        public UnityEvent OnChomped;

        [SerializeField]
        public UnityEvent OnDebugEscape;

        private void Start()
        {
            InputService.Instance.OnChomped.AddListener(HandleChompInput);
            InputService.Instance.OnLeftStridePressed.AddListener(HandleLeftStrideInput);
            InputService.Instance.OnRightStridePressed.AddListener(HandleRightStrideInput);
            InputService.Instance.OnDebugEscape.AddListener(HandleDebugEscape);
        }

        private void OnDestroy()
        {
            InputService.Instance.OnChomped.RemoveListener(HandleChompInput);
            InputService.Instance.OnLeftStridePressed.RemoveListener(HandleLeftStrideInput);
            InputService.Instance.OnRightStridePressed.RemoveListener(HandleRightStrideInput);
            InputService.Instance.OnDebugEscape.RemoveListener(HandleDebugEscape);
        }

        public void HandleLeftStrideInput()
        {
            OnLeftStridePressed?.Invoke();
        }

        public void HandleRightStrideInput()
        {
            OnRightStridePressed?.Invoke();
        }

        public void HandleChompInput()
        {
            OnChomped?.Invoke();
        }

        public void HandleDebugEscape()
        {
            OnDebugEscape?.Invoke();
        }
    }
}