using Unity.Cinemachine;
using UnityEngine;

namespace Feel
{
    [CreateAssetMenu(fileName = "ScreenShakeSO")]
    public class ScreenShakeSO : ScriptableObject
    {
        [field: SerializeField]
        public CinemachineImpulseDefinition ImpulseDefinition { get; private set; }

        public void GenerateImpulse(Vector2 position, Vector2 velocity)
        {
            ImpulseDefinition.CreateEvent(position, velocity);
        }
    }
}