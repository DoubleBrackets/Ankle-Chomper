using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    public class ArduinoInputMono : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onButtonPressed;

        private void Start()
        {
            ArduinoInput.Instance.OnButtonPressed.AddListener(OnButtonPressed);
        }

        private void OnDestroy()
        {
            ArduinoInput.Instance.OnButtonPressed.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            onButtonPressed?.Invoke();
        }
    }
}