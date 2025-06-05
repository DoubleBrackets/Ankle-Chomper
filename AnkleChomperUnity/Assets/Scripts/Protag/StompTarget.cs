using UnityEngine;
using UnityEngine.Events;

namespace Protag
{
    public class StompTarget : MonoBehaviour
    {
        public UnityEvent OnStomped;

        private bool isStomped;

        public void Stomp()
        {
            if (isStomped)
            {
                return;
            }

            isStomped = true;
            OnStomped?.Invoke();
        }
    }
}