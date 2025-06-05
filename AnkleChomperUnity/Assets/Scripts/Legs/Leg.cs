using UnityEngine;
using UnityEngine.Events;

namespace Legs
{
    public class Leg : MonoBehaviour
    {
        [SerializeField]
        private Transform chompTarget;

        [Header("Events")]

        public UnityEvent OnEaten;

        public UnityEvent OnTargeted;
        public UnityEvent OnUnTargeted;

        private bool eaten;

        public Vector3 GetChompTargetPosition()
        {
            // Return the position of the leg for chomping
            return chompTarget.position;
        }

        public bool IsValidTarget()
        {
            return chompTarget != null && !eaten;
        }

        public void Eaten()
        {
            if (eaten)
            {
                return;
            }

            eaten = true;
            // Trigger the eaten event
            OnEaten?.Invoke();

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
    }
}