using UnityEngine;
using UnityEngine.Events;

namespace Legs
{
    public class Leg : MonoBehaviour
    {
        private enum LegState
        {
            Idle,
            Eaten,
            Stomping
        }

        [SerializeField]
        private Transform chompTarget;

        [Header("Events")]

        public UnityEvent<Leg> OnEaten;

        public UnityEvent OnTargeted;
        public UnityEvent OnUnTargeted;

        public UnityEvent OnStompEffects;

        private LegState state = LegState.Idle;

        public Vector3 GetChompTargetPosition()
        {
            // Return the position of the leg for chomping
            return chompTarget.position;
        }

        public bool IsValidTarget()
        {
            return chompTarget != null && state == LegState.Idle;
        }

        public void Eaten()
        {
            if (state != LegState.Idle)
            {
                return;
            }

            state = LegState.Eaten;
            // Trigger the eaten event
            OnEaten?.Invoke(this);

            // Optionally, destroy the leg after being eaten
            Destroy(gameObject, 3f);
        }

        public void SetTargeted(bool isInRange)
        {
            if (isInRange)
            {
                OnTargeted?.Invoke();
            }
            else
            {
                OnUnTargeted?.Invoke();
            }
        }

        public void DoStompEffects()
        {
            OnStompEffects?.Invoke();
        }
    }
}