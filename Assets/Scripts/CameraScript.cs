using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    
    private void Update()
    {
        transform.RotateAround(target.position, rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
