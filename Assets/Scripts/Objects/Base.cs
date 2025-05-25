using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Material teamMaterial;
    public Material TeamMaterial => teamMaterial;
    
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform dockPosition;
    
    public int ResAmount { get; private set; }
    public Transform DockPosition => dockPosition;
    
    private DroneManager droneManager;

    private void Awake()
    {
        droneManager = GetComponent<DroneManager>();
        GetComponent<MeshRenderer>().material = teamMaterial;
    }

    private void Start()
    {
        droneManager.SpawnDrone(spawnPosition);
    }
    
    public void AddResource()
    {
        ResAmount++;
    }
}
