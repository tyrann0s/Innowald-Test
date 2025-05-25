using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DroneManager : MonoBehaviour
    {
        [SerializeField] private GameObject dronePrefab;

        public Base ParentBase { get; private set; }
        public ResourceManager ResManager { get; private set; }
        
        private List<Drone> dronePool;

        private int poolSize = 1;
        public int PoolSize
        {
            get => poolSize;
            set
            {
                if (poolSize != value)
                {
                    poolSize = value;
                    OnDronePoolChanged();
                }
            }
        }
        
        private float dronesSpeed = 1f;
        public bool TraceLine { get; set; }

        private void OnDronePoolChanged()
        {
            if (dronePool.Count < PoolSize)
            {
                int dronesToSpawn = PoolSize - dronePool.Count;
                for (int i = 0; i < dronesToSpawn; i++)
                {
                    SpawnDrone();
                }
            }
            else if (dronePool.Count > PoolSize)
            {
                int dronesToDestroy = dronePool.Count - PoolSize;
                for (int i = 0; i < dronesToDestroy; i++)
                {
                    Drone drone = dronePool[dronePool.Count - 1];
                    dronePool.RemoveAt(dronePool.Count - 1);
                    Destroy(drone.gameObject);
                }
            }
        }

        private void Awake()
        {
            ParentBase = GetComponentInParent<Base>();
            ResManager = FindFirstObjectByType<ResourceManager>();
            dronePool = new List<Drone>();
        }

        private void Start()
        {
            SpawnDrone();
        }

        public void SpawnDrone()
        {
            GameObject droneObject = Instantiate(dronePrefab, ParentBase.SpawnPosition.position, ParentBase.SpawnPosition.rotation);
            var drone = droneObject.GetComponent<Drone>();
            dronePool.Add(drone);
            drone.SetUpDrone(this, dronesSpeed, TraceLine);
        }
        
        public void ChangeDronesSpeed(float speed)
        {
            dronesSpeed = speed;
            foreach (var drone in dronePool)
            {
                drone.MoveSpeed = dronesSpeed;
            }
        }
        
        public void ChangeDronesTraceLine(bool value)
        {
            TraceLine = value;
            foreach (var drone in dronePool)
            {
                drone.ToggleTraceLine(TraceLine);
            }
        }
    }
}