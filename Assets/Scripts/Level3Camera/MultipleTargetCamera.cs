using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    
    // Separate smoothing controls
    public float positionSmoothTime = 0.5f;
    public float zoomSmoothTime = 0.5f;
    
    public float minSize = 5f;
    public float padding = 2f;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true; 
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, positionSmoothTime);
    }

    void Zoom()
    {
        float requiredSize = GetRequiredSize();
        float targetSize = Mathf.Max(requiredSize, minSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSmoothTime * Time.deltaTime);
    }
    
    float GetRequiredSize()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        float sizeX = bounds.size.x * 0.5f / cam.aspect;
        float sizeY = bounds.size.y * 0.5f;

        return Mathf.Max(sizeX, sizeY) + padding;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}