using System.Collections.Generic;
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
        public UnityEvent OnDebugEscape;
        public static InputService Instance { get; private set; }

        private HashSet<object> _inputBlockers = new();

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
            if (_inputBlockers.Count > 0)
            {
                return;
            }

            if (context.started)
            {
                OnLeftStridePressed?.Invoke();
            }
        }

        public void TriggerRightStridePressed(InputAction.CallbackContext context)
        {
            if (_inputBlockers.Count > 0)
            {
                return;
            }

            if (context.started)
            {
                OnRightStridePressed?.Invoke();
            }
        }

        public void DebugEscapePressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnDebugEscape?.Invoke();
            }
        }

        public void TriggerChomped()
        {
            if (_inputBlockers.Count > 0)
            {
                return;
            }

            OnChomped?.Invoke();
        }

        public void TriggerChomped(InputAction.CallbackContext context)
        {
            if (_inputBlockers.Count > 0)
            {
                return;
            }

            if (context.started)
            {
                OnChomped?.Invoke();
            }
        }

        public void AddInputBlocker(object key)
        {
            _inputBlockers.Add(key);
        }

        public void RemoveInputBlocker(object key)
        {
            _inputBlockers.Remove(key);
        }
    }
}