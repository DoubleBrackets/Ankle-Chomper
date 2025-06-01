using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    public class ProtagInput : MonoBehaviour
    {
        [Header("Events")]

        [SerializeField]
        public UnityEvent OnLeftStridePressed;

        [SerializeField]
        public UnityEvent OnRightStridePressed;

        [SerializeField]
        public UnityEvent OnChomped;

        public void HandleLeftStrideInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnLeftStridePressed?.Invoke();
            }
        }

        public void HandleRightStrideInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnRightStridePressed?.Invoke();
            }
        }

        public void HandleChompInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnChomped?.Invoke();
            }
        }
    }
}