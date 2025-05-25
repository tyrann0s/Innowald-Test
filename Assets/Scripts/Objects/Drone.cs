using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float MoveSpeed { get; set; }
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float avoidanceRadius = 2f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float harvestTime = 2f;
    
    [SerializeField] private TextMeshPro stateText;
    private LineRenderer lineRenderer;
    
    private GameObject targetResource;
    private DroneManager droneManager;
    private Vector3 currentDirection;
    
    private bool haveResource;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpDrone(DroneManager droneManager, float speed, bool traceLine)
    {
        MoveSpeed = speed;
        this.droneManager = droneManager;
        GetComponentInChildren<MeshRenderer>().material = droneManager.ParentBase.TeamMaterial;
        lineRenderer.enabled = traceLine;
        SetText();
    }
    
    private void Update()
    {
        stateText.transform.rotation = Camera.main.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (targetResource == null)
        {
            targetResource = FindNearestResource();
        }

        if (targetResource != null)
        {
            Vector3 targetDirection = (targetResource.transform.position - transform.position).normalized;
            currentDirection = AvoidObstacles(targetDirection);
            transform.position += currentDirection * (MoveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentDirection), MoveSpeed * Time.deltaTime);
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetResource.transform.position);
        }
    }

    private Vector3 AvoidObstacles(Vector3 targetDirection)
    {
        Vector3 avoidanceDirection = targetDirection;
        Collider[] obstacles = Physics.OverlapSphere(transform.position, avoidanceRadius, obstacleLayer);
        
        if (obstacles.Length > 0)
        {
            Vector3 avoidance = Vector3.zero;
            foreach (var obstacle in obstacles)
            {
                avoidance += (transform.position - obstacle.transform.position).normalized;
            }
            avoidanceDirection = (targetDirection + avoidance.normalized).normalized;
        }
        
        return avoidanceDirection;
    }

    private GameObject FindNearestResource()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        GameObject nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Resource"))
            {
                Resource resource = collider.GetComponent<Resource>();
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance && !resource.IsOccupied)
                {
                    nearestDistance = distance;
                    nearest = collider.gameObject;
                }
            }
        }
        if (nearest != null) nearest.GetComponent<Resource>().IsOccupied = true;
        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            if (!haveResource) StartCoroutine(HarvestResource());
        }
        
        if (other.CompareTag("Dock"))
        {
            if (haveResource) StartCoroutine(UnloadResource());
        }
    }

    private IEnumerator HarvestResource()
    {
        haveResource = true;
        yield return new WaitForSeconds(harvestTime);
        droneManager.ResManager.ReturnToPool(targetResource);
        targetResource = droneManager.ParentBase.DockPosition.gameObject;
        SetText();
    }
    
    private IEnumerator UnloadResource()
    {
        haveResource = false;
        yield return new WaitForSeconds(harvestTime);
        
        droneManager.ParentBase.AddResource();
        targetResource = null;
        SetText();
    }

    private void SetText()
    {
        if (haveResource) stateText.text = "RETURNING TO DOCK";
        else stateText.text = "HARVESTING";
    }

    public void ToggleTraceLine(bool value)
    {
        lineRenderer.enabled = value;
    }
}
