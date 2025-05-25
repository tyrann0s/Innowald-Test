using System;
using Managers;
using UI;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Material teamMaterial;
    public Material TeamMaterial => teamMaterial;

    [SerializeField] private GameObject building;
    [SerializeField] private ParticleSystem resVFX;
    
    [SerializeField] private Transform spawnPosition;
    public Transform SpawnPosition => spawnPosition;
    [SerializeField] private Transform dockPosition;
    public Transform DockPosition => dockPosition;
    
    [SerializeField] private UIPanel teamPanel;

    private int resAmount;

    private DroneManager droneManager;
    public DroneManager DroneManager => droneManager;

    private void Awake()
    {
        droneManager = GetComponent<DroneManager>();
        building.GetComponent<MeshRenderer>().material = teamMaterial;
        
        teamPanel.ParentBase = this;
    }
    
    public void AddResource()
    {
        resAmount++;
        resVFX.Play();
        teamPanel.UpdateResAmount(resAmount);
    }
}
