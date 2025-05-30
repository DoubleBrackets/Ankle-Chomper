using Legs;
using UnityEngine;

namespace Systems
{
    public class LegSpawner : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private Leg _legPrefab;

        [SerializeField]
        private Transform _spawnParent;

        [Header("Config")]

        [SerializeField]
        private int _numberOfLegs;

        [SerializeField]
        private float _spawnRadius;

        private void Start()
        {
            SpawnLegs();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }

        private void SpawnLegs()
        {
            for (var i = 0; i < _numberOfLegs; i++)
            {
                Vector2 pos = Random.insideUnitCircle * _spawnRadius;
                var spawnPos = new Vector3(pos.x, 0, pos.y);

                Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                Leg leg = Instantiate(_legPrefab, spawnPos, rot, transform);
            }
        }
    }
}