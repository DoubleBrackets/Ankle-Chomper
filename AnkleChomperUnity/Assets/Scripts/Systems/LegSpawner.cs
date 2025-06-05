using System.Collections.Generic;
using Legs;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private readonly List<Leg> _currentLegs = new();

        private void Start()
        {
            SpawnInitialLegs();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                SpawnInitialLegs();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }

        private void SpawnInitialLegs()
        {
            for (var i = 0; i < _numberOfLegs; i++)
            {
                SpawnLeg();
            }
        }

        private void SpawnLeg()
        {
            Vector2 pos = Random.insideUnitCircle * _spawnRadius;
            var spawnPos = new Vector3(pos.x, 0, pos.y);

            Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            Leg leg = Instantiate(_legPrefab, spawnPos, rot, transform);

            _currentLegs.Add(leg);

            leg.OnEaten.AddListener(OnLegEaten);
        }

        private void OnLegEaten(Leg leg)
        {
            leg.OnEaten.RemoveListener(OnLegEaten);
            _currentLegs.Remove(leg);
            SpawnLeg();
        }
    }
}