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

        private void Start()
        {
            InputService.Instance.OnChomped.AddListener(HandleChompInput);
            InputService.Instance.OnLeftStridePressed.AddListener(HandleLeftStrideInput);
            InputService.Instance.OnRightStridePressed.AddListener(HandleRightStrideInput);
        }

        private void OnDestroy()
        {
            InputService.Instance.OnChomped.RemoveListener(HandleChompInput);
            InputService.Instance.OnLeftStridePressed.RemoveListener(HandleLeftStrideInput);
            InputService.Instance.OnRightStridePressed.RemoveListener(HandleRightStrideInput);
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
    }
}