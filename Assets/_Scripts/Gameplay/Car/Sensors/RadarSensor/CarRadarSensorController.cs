using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarRadarSensorController : MonoBehaviour
{
    public Action<float> OnObjectSpeedInfoRequested;
    
    public Transform raycastOrigin; // The position from which the raycast is shot
    public LayerMask detectionLayer; // The layer mask to detect objects
    public float maxDetectionDistance = 50f; // Maximum distance to detect objects

    public bool debugMode = false;
    private GameObject closestObject = null; // Reference to the closest detected object
    private ObjectSpecificationModel currentFrameObjectSpecification = null; // Information about the detected object in the current frame

    private void Update()
    {
        ShootRaycast();
        CalculateObjectSpeed();
    }

    private void ShootRaycast()
    {
        RaycastHit hit;

        // Cast a ray forward from the raycast origin
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, maxDetectionDistance, detectionLayer))
        {
            closestObject = hit.collider.gameObject;

            if (currentFrameObjectSpecification == null)
            {
                currentFrameObjectSpecification = new ObjectSpecificationModel(closestObject.transform.position, Time.deltaTime);
            }
            else
            {
                currentFrameObjectSpecification.AddLastPosition(closestObject.transform.position, Time.deltaTime);
            }
        }
        else
        {
            // Reset the closest object if no object is detected
            closestObject = null;
            currentFrameObjectSpecification = null;
        }
    }

    private void CalculateObjectSpeed()
    {
        if (currentFrameObjectSpecification != null)
        {
            // Calculate average speed based on the list of radar position data
            float totalDistance = 0f;
            float totalTime = 0f;
            int count = currentFrameObjectSpecification.positions.Count;

            for (int i = 1; i < count; i++)
            {
                Vector3 previousPosition = currentFrameObjectSpecification.positions[i - 1].position;
                Vector3 currentPosition = currentFrameObjectSpecification.positions[i].position;
                float deltaTime = currentFrameObjectSpecification.positions[i].timeAtThisPosition;

                totalDistance += Vector3.Distance(previousPosition, currentPosition);
                totalTime += deltaTime;
            }

            if (totalTime > 0f)
            {
                float averageSpeed = totalDistance / totalTime;
                currentFrameObjectSpecification.SetSpeed(averageSpeed);
                
                if (OnObjectSpeedInfoRequested != null)
                    OnObjectSpeedInfoRequested(averageSpeed);
                
                if(debugMode)
                    Debug.Log($"{closestObject.name}, is moving at an average speed of -> {averageSpeed}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (raycastOrigin == null)
            return;

        Gizmos.color = Color.red;
        var targetPos = raycastOrigin.transform.position;
        targetPos.z *= maxDetectionDistance;
        Gizmos.DrawLine(raycastOrigin.transform.position, targetPos);
    }
}

[System.Serializable]
public class ObjectSpecificationModel
{
    public List<RadarPositionData> positions; // Last recorded position of the object
    private float _speed; // Speed of the object

    public ObjectSpecificationModel(Vector3 newPos, float time)
    {
        positions = new List<RadarPositionData>();
        AddLastPosition(newPos, time);
    }

    public void AddLastPosition(Vector3 newPosition, float time)
    {
        if(positions == null)
            positions = new List<RadarPositionData>();
        
        positions.Insert(0, new RadarPositionData()
        {
            position = newPosition,
            timeAtThisPosition = time,
        });

        // Remove the oldest position if the list exceeds the maximum capacity
        if (positions.Count > 50)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public float GetSpeed()
    {
        return _speed;
    }
}

[System.Serializable]
public class RadarPositionData
{
    public Vector3 position;
    public float timeAtThisPosition;
}
