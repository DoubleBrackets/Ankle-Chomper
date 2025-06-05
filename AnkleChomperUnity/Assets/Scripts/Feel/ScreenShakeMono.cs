using Feel;
using UnityEngine;

namespace Camera
{
    public class ScreenShakeMono : MonoBehaviour
    {
        [SerializeField]
        private ScreenShakeSO screenShakeSO;

        [SerializeField]
        private Vector2 velocity;

        [SerializeField]
        private float strength;

        [ContextMenu("Generate Impulse")]
        public void GenerateImpulse()
        {
            screenShakeSO.GenerateImpulse(transform.position, velocity * strength);
        }
        
        [ContextMenu("Generate Random Impulse")]
        public void GenerateRandomImpulse()
        {
            Vector2 randomVelocity = Random.insideUnitCircle.normalized * velocity.magnitude;
            screenShakeSO.GenerateImpulse(transform.position, randomVelocity * strength);
        }
    }
}