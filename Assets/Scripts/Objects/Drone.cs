using System;
using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float avoidanceRadius = 2f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float harvestTime = 2f;
    
    private GameObject targetResource;
    private DroneManager droneManager;
    private Vector3 currentDirection;
    
    public void SetUpDrone(DroneManager droneManager)
    {
        this.droneManager = droneManager;
        GetComponentInChildren<MeshRenderer>().material = droneManager.ParentBase.TeamMaterial;
    }

    private void Update()
    {
        if (targetResource == null)
        {
            targetResource = FindNearestResource();
        }

        if (targetResource != null)
        {
            Vector3 targetDirection = (targetResource.transform.position - transform.position).normalized;
            currentDirection = AvoidObstacles(targetDirection);
            transform.position += currentDirection * (moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(currentDirection);
            transform.forward = currentDirection;
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
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = collider.gameObject;
                }
            }
        }
        
        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            StartCoroutine(HarvestResource());
        }
        
        if (other.CompareTag("Dock"))
        {
            StartCoroutine(UnloadResource());
        }
    }

    private IEnumerator HarvestResource()
    {
        yield return new WaitForSeconds(harvestTime);
        droneManager.ResManager.ReturnToPool(targetResource);
        targetResource = droneManager.ParentBase.DockPosition.gameObject;
    }
    
    private IEnumerator UnloadResource()
    {
        yield return new WaitForSeconds(harvestTime);
        droneManager.ParentBase.AddResource();
        targetResource = null;
    }
}
