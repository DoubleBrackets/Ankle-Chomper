using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputService : MonoBehaviour
    {
        [Header("Events")]

        public UnityEvent OnLeftStridePressed;

        public UnityEvent OnRightStridePressed;

        public UnityEvent OnChomped;
        public static InputService Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TriggerLeftStridePressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnLeftStridePressed?.Invoke();
            }
        }

        public void TriggerRightStridePressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnRightStridePressed?.Invoke();
            }
        }

        public void TriggerChomped()
        {
            OnChomped?.Invoke();
        }

        public void TriggerChomped(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnChomped?.Invoke();
            }
        }
    }
}