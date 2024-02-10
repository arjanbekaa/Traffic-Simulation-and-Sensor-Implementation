using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProximitySensorModel
{
    public string sensorName;
    public ProximitySensorType proximitySensorType;
    public Transform sensorTransform;
    public float sensorDistance;
    public float sensorAngle = 30f; // Angle in degrees
    public int rayCount = 5; // Number of rays to fire
    public AudioSource audioSource;

    [Range(0.01f, 0.5f)]
    public float hitDistanceOffset = 0.5f;
    public Gradient colorGradient;
    public GameObject feedbackSensorsHolder3D;
    public List<MeshRenderer> feedbackSensorsRenderers;
    
    private bool collisionDetected;
    private float closestHitDistance = float.MaxValue;
    public Color gizmosColor = Color.green;

    private CarManager _carManager;

    public void Init(CarManager carManager)
    {
        _carManager = carManager;
    }
    
    public ProximitySensorResult CheckCollision()
    {
        ProximitySensorResult proximitySensorResults = new ProximitySensorResult();
        
        collisionDetected = false;
        closestHitDistance = float.MaxValue;
        // Calculate the angle increment between rays
        float angleIncrement = sensorAngle / (rayCount - 1);

        // Iterate through each ray
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the current angle for the ray
            float currentAngle = -sensorAngle / 2 + i * angleIncrement;

            // Calculate the direction of the current ray
            Vector3 rayDirection = Quaternion.Euler(0, currentAngle, 0) * sensorTransform.forward;

            // Cast a ray in the current direction
            Ray ray = new Ray(sensorTransform.position, rayDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, sensorDistance))
            {
                //if (hit.transform.gameObject.layer == 9)
                //    continue;
                
                // Collision detected
                var dist = Vector3.Distance(sensorTransform.position, hit.transform.position);
                
                if(dist < closestHitDistance)
                    closestHitDistance = dist - hitDistanceOffset;
                
                collisionDetected = true;
            }

            if(_carManager.carProximitySensorController.debugLines)
                DrawLines(i, collisionDetected);
        }

        UpdateSensorsFeedback(closestHitDistance, collisionDetected);

        proximitySensorResults.sensorType = proximitySensorType;
        proximitySensorResults.didHit = collisionDetected;
        proximitySensorResults.closestHitDistance = closestHitDistance;
        
        return proximitySensorResults;
    }

    void PlayCollisionAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void UpdateSensorsFeedback(float hitDistance, bool colDetected)
    {
        if(feedbackSensorsRenderers == null || feedbackSensorsRenderers.Count == 0)
            return;
        
        float closestDist = float.MaxValue;
        
        for (int i = 0; i < feedbackSensorsRenderers.Count; i++)
        {
            float distanceToRenderer = Vector3.Distance(sensorTransform.position, feedbackSensorsRenderers[i].transform.position);

            float normalizedDistance = Mathf.Clamp01(distanceToRenderer / hitDistance);

            if (normalizedDistance <= closestDist)
                closestDist = normalizedDistance;

            feedbackSensorsRenderers[i].material.color = colorGradient.Evaluate(normalizedDistance);
        }

        if (colDetected)
        {
            _carManager.carProximitySensorSoundController.PlaySound(closestDist, audioSource);
        }
        else
        {
            audioSource.Stop();
        }
    }
    
    public void DrawLines(int rayIndex, bool didHit)
    {
        float angleIncrement = sensorAngle / (rayCount - 1);

        float currentAngle = -sensorAngle / 2 + rayIndex * angleIncrement;

        Vector3 rayDirection = Quaternion.Euler(0, currentAngle, 0) * sensorTransform.forward;

        Vector3 lineEndPosition = sensorTransform.position + rayDirection * sensorDistance;

        if (didHit)
        {
            Debug.DrawLine(sensorTransform.position, lineEndPosition, Color.red);
        }
        else
        {
            Debug.DrawLine(sensorTransform.position, lineEndPosition, Color.green);
        }
    }
    
    public void DrawGizmos()
    {
        for (int i = 0; i < rayCount; i++)
        {
            float angleIncrement = sensorAngle / (rayCount - 1);

            float currentAngle = -sensorAngle / 2 + i * angleIncrement;

            Vector3 rayDirection = Quaternion.Euler(0, currentAngle, 0) * sensorTransform.forward;

            Gizmos.color = gizmosColor;
            Gizmos.DrawRay(sensorTransform.position, rayDirection * sensorDistance);
        }
    }
}

[System.Serializable]
public enum ProximitySensorType
{
    SensorFrontCenter,
    SensorFrontLeft,
    SensorFrontRight,
    SensorFrontLeftSide,
    SensorBehindLeftSide,
    SensorFrontRightSide,
    SensorBehindRightSide,
    SensorBehindCenter,
    SensorBehindLeft,
    SensorBehindRight,
}

[System.Serializable]
public class ProximitySensorResult
{
    public ProximitySensorType sensorType;
    public float closestHitDistance;
    public bool didHit;
}
