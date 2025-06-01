using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Protag
{
    public class ProtagController : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private List<ProtagState> _states;

        [ShowNonSerializedField]
        private ProtagState _currentState;

        private void Awake()
        {
            foreach (ProtagState state in _states)
            {
                state.Initialize(this);
            }

            SwitchToDefaultState();
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUILayout.Label($"Current State: {(_currentState != null ? _currentState.GetType().Name : "None")}");
#endif
        }

        [Button("Autodetect states")]
        private void AutoDetectStates()
        {
            _states = new List<ProtagState>(GetComponentsInChildren<ProtagState>());
        }

        public void SwitchToDefaultState()
        {
            if (_states.Count == 0)
            {
                return;
            }

            SwitchToState(_states[0]);
        }

        public void SwitchToState(ProtagState newState)
        {
            if (_currentState != null)
            {
                _currentState.OnExitState();
            }

            _currentState = newState;

            if (_currentState != null)
            {
                _currentState.EnterState();
            }
            else
            {
                Debug.LogError("Attempted to switch to a null state.");
            }
        }
    }
}