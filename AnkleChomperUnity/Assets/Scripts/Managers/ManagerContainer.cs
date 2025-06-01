using UnityEngine;

namespace Systems
{
    public class ManagerContainer : MonoBehaviour
    {
        private static ManagerContainer _instance;

        [SerializeField]
        private GameObject _managerPrefab;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                if (_managerPrefab != null)
                {
                    Instantiate(_managerPrefab, transform);
                }

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}