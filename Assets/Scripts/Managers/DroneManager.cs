using System;
using Managers;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab;
    public Base ParentBase { get; private set; }
    public ResourceManager ResManager { get; private set; }

    private void Awake()
    {
        ParentBase = GetComponentInParent<Base>();
        ResManager = FindFirstObjectByType<ResourceManager>();
    }

    public void SpawnDrone(Transform spawnPosition)
    {
        var drone = Instantiate(dronePrefab, spawnPosition.position, spawnPosition.rotation).GetComponent<Drone>();
        drone.SetUpDrone(this);
    }
}
