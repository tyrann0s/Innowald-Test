using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject resourcePrefab;
    
        [SerializeField]
        private int poolSize = 20;
    
        private List<GameObject> pool;

        public float ResourceSpawnDelay { get; set; } = 5f;

        [SerializeField] private float spawnRadius;
        [SerializeField] private float minDistanceFromOthers = 2f;
        [SerializeField] private int maxSpawnAttempts = 10;
    
        private void Awake()
        {
            pool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(resourcePrefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnResource());
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        private IEnumerator SpawnResource()
        {
            while (true)
            {
                Vector3 spawnPoint = Vector3.zero;
                bool validPointFound = false;
        
                for (int attempt = 0; attempt < maxSpawnAttempts && !validPointFound; attempt++)
                {
                    Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
                    if (Physics.OverlapSphere(randomPoint, minDistanceFromOthers).Length == 0)
                    {
                        spawnPoint = randomPoint;
                        validPointFound = true;
                    }
                }
        
                if (validPointFound)
                {
                    GameObject resource = GetFromPool();
                    resource.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z);
                }
        
                yield return new WaitForSeconds(ResourceSpawnDelay);
            }
        }

        public GameObject GetFromPool()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive(true);
                    return pool[i];
                }
            }
        
            GameObject obj = Instantiate(resourcePrefab);
            pool.Add(obj);
            return obj;
        }
    
        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}
