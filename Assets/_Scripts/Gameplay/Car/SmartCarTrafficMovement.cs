using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SmartCarTrafficMovement : MonoBehaviour
{
    public List<RoadLane> roadLanes;
    public float laneChangeSpeed = 2f;
    public float maxSpeed = 10f;
    
    private float _currentCarSpeed = 10f;

    [Header("Lane Change Config")] 
    [Range(0f,5f)]
    public float stoppingDistance = 2f;
    [Range(0f,10f)]
    public float behindDistanceNeededToChangeLane = 3;
    [Range(0f,10f)]
    public float frontDistanceNeededToChangeLane = 3;
    
    private RoadLaneType _currentRoadLaneType = RoadLaneType.CenterLane;
    private Transform frontSensor;

    private bool isChangingLane = false;
    
    public bool debuggingMode = false;

    private float frontDetectedObjectSpeed;

    private CarManager _carManager;
    private void Start()
    {
        _carManager = GetComponentInParent<CarManager>();
        _carManager.carProximitySensorController.OnProximitySensorResultAction += OnSensorProximitySensorResult;
        _carManager.carRadarSensorController.OnObjectSpeedInfoRequested += OnObjectSpeedInfoUpdated;
    }
    
    private void OnObjectSpeedInfoUpdated(float targetSpeed)
    {
        frontDetectedObjectSpeed = targetSpeed - 0.5f;
    }

    private void Update()
    {
        MoveForward();
        ChangeLaneMovement();
    }

    public void OnSensorProximitySensorResult(List<ProximitySensorResult> proximitySensorResults)
    {
        if(isChangingLane)
            return;
        
        var frontCenterProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorFrontCenter);

        if (frontCenterProximitySensorResult.closestHitDistance <= stoppingDistance)
        {
            StopTheCar();
        }
        
        if (!frontCenterProximitySensorResult.didHit)
        {
            ChangeCurrentSpeedGradually(maxSpeed, 0.5f);
            if(debuggingMode)
                Debug.Log($"{this.name}, did Not Hit Anything");
        }
        else
        {
            if(debuggingMode)
                Debug.Log($"{this.name}, can not move forward and currently is in the lane -> {_currentRoadLaneType}");

            SlowDown();
            
            RoadLaneType targetLane = RoadLaneType.LeftLane;
            bool didFindFreeLane = false;
            
            switch (_currentRoadLaneType)
            {
                case RoadLaneType.CenterLane:

                    didFindFreeLane = CheckIfLeftLaneFree(proximitySensorResults);

                    if (!didFindFreeLane)
                    {
                        targetLane = RoadLaneType.RightLane;
                        didFindFreeLane = CheckIfRightLaneFree(proximitySensorResults);
                    }

                    if (didFindFreeLane)
                    {
                        ChangeLane(targetLane);
                    }
                    else
                    {
                        if (frontCenterProximitySensorResult.closestHitDistance <= stoppingDistance)
                        {
                            StopTheCar();
                        }
                        
                        SlowDown();
                    }
                    
                    break;
                case RoadLaneType.RightLane:
                            
                    targetLane = RoadLaneType.CenterLane;
                    didFindFreeLane = CheckIfCenterLaneFree(proximitySensorResults);

                    if (didFindFreeLane)
                    {
                        ChangeLane(targetLane);
                    }
                    else
                    {
                        if (frontCenterProximitySensorResult.closestHitDistance <= stoppingDistance)
                        {
                            SlowDown();
                        }
                    }

                    break;
                case RoadLaneType.LeftLane:
                    
                    targetLane = RoadLaneType.CenterLane;
                    didFindFreeLane = CheckIfCenterLaneFree(proximitySensorResults);

                    if (didFindFreeLane)
                    {
                        ChangeLane(targetLane);
                    }
                    else
                    {
                        if (frontCenterProximitySensorResult.closestHitDistance <= stoppingDistance)
                        {
                            SlowDown();
                        }
                    }
                    
                    break;
            } 
        }
    }

    public bool CheckIfRightLaneFree(List<ProximitySensorResult> proximitySensorResults)
    {
        if(debuggingMode)
            Debug.Log($"{this.name}, is checking if it can move to the -> {RoadLaneType.RightLane}");
        
        bool result = false;
        
        var frontRightProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorFrontRight);
        var frontRightSideProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorFrontRightSide);
        var behindRightSideProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorBehindRightSide);
        var behindRightProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorBehindRight);

        bool isLeftSideFree = !frontRightSideProximitySensorResult.didHit || !behindRightSideProximitySensorResult.didHit;

        if(debuggingMode)
            Debug.Log($"{this.name}, is right side free -> {isLeftSideFree}");
        
        if (isLeftSideFree)
        {
            bool isRightForwardFree = !frontRightProximitySensorResult.didHit;
            bool isRightBehindFree = !behindRightProximitySensorResult.didHit;

            if (isRightForwardFree && isRightBehindFree)
            {
                result = true;
            }
            else
            {
                if (!isRightForwardFree)
                {
                    isRightForwardFree = frontRightProximitySensorResult.closestHitDistance >= frontDistanceNeededToChangeLane;
                }

                if (!isRightBehindFree)
                {
                    isRightBehindFree = behindRightProximitySensorResult.closestHitDistance >= behindDistanceNeededToChangeLane;
                }
                
                if (isRightForwardFree && isRightBehindFree)
                {
                    result = true;
                }
            }
        }
        
        if(debuggingMode)
            Debug.Log($"{this.name}, can move to the right -> {result}");
        
        return result;
    }
    
    public bool CheckIfLeftLaneFree(List<ProximitySensorResult> proximitySensorResults)
    {
        if(debuggingMode)
            Debug.Log($"{this.name}, is checking if it can move to the -> {RoadLaneType.RightLane}");
        
        bool result = false;
        
        var frontLeftProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorFrontLeft);
        var frontLeftSideProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorFrontLeftSide);
        var behindLeftSideProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorBehindLeftSide);
        var behindLeftProximitySensorResult =
            proximitySensorResults.Find(x => x.sensorType == ProximitySensorType.SensorBehindLeft);

        bool isLeftSideFree = !frontLeftSideProximitySensorResult.didHit || !behindLeftSideProximitySensorResult.didHit;

        if(debuggingMode)
            Debug.Log($"{this.name}, is left side free -> {isLeftSideFree}");
        
        if (isLeftSideFree)
        {
            bool isLeftForwardFree = !frontLeftProximitySensorResult.didHit;
            bool isLeftBehindFree = !behindLeftProximitySensorResult.didHit;

            if (isLeftForwardFree && isLeftBehindFree)
            {
                result = true;
            }
            else
            {
                if (!isLeftForwardFree)
                {
                    isLeftForwardFree = frontLeftProximitySensorResult.closestHitDistance >= frontDistanceNeededToChangeLane;
                }

                if (!isLeftBehindFree)
                {
                    isLeftBehindFree = behindLeftProximitySensorResult.closestHitDistance >= behindDistanceNeededToChangeLane;
                }
                
                if (isLeftForwardFree && isLeftBehindFree)
                {
                    result = true;
                }
            }
        }
        
        if(debuggingMode)
            Debug.Log($"{this.name}, can move to the left -> {result}");

        return result;
    }
    
    public bool CheckIfCenterLaneFree(List<ProximitySensorResult> proximitySensorResults)
    {
        if(debuggingMode)
            Debug.Log($"{this.name}, is checking if it can move to the -> {RoadLaneType.CenterLane}");

        bool result = false;
        
        if (_currentRoadLaneType == RoadLaneType.LeftLane)
        {
            result = CheckIfRightLaneFree(proximitySensorResults);
        }
        else if (_currentRoadLaneType == RoadLaneType.RightLane)
        {
            result = CheckIfLeftLaneFree(proximitySensorResults);
        }

        return result;
    }

    public void ChangeLane(RoadLaneType laneType)
    {
        SlowDown();
        isChangingLane = true;
        _currentRoadLaneType = laneType;
        laneChangeStartTime = Time.time;
    }
    
    private void MoveForward()
    {
        if(debuggingMode)
            Debug.Log($"{this.name}, is moving forward");
        
        // Move forward at a constant speed
        transform.Translate(Vector3.forward * _currentCarSpeed * Time.deltaTime);
    }

    private float laneChangeStartTime; // Timestamp when the lane change started

    private void ChangeLaneMovement()
    {
        if (isChangingLane)
        {
            if (debuggingMode)
                Debug.Log($"{this.name}, is changing forward");

            var targetLane = roadLanes.Find(x => x.roadLaneType == _currentRoadLaneType);
            Vector3 targetPosition = transform.position;

            // Calculate the direction towards the target lane
            Vector3 directionToTarget = (targetLane.roadLanePosition - transform.position).normalized;

            // Calculate the distance to move based on laneChangeSpeed and the time elapsed since the lane change started
            float timeElapsed = Time.time - laneChangeStartTime;
            float distanceToMove = laneChangeSpeed * timeElapsed;

            // Move towards the target lane position
            targetPosition += directionToTarget * distanceToMove;

            // Check if we've reached or passed the target lane position
            if (Mathf.Abs(transform.position.x - targetLane.roadLanePosition.x) <= 0.01f)
            {
                targetPosition = targetLane.roadLanePosition;
                isChangingLane = false;
                _currentCarSpeed = maxSpeed;
                
                if(debuggingMode)
                    Debug.Log($"{this.name}, finished lane change");
            }

            targetPosition.z = transform.position.z;
            transform.position = targetPosition;
        }
    }

    public void ChangeCurrentSpeed(float targetSpeed)
    {
        _currentCarSpeed = targetSpeed;
    }
    
    private void StopTheCar()
    {
        if(debuggingMode)
            Debug.Log($"{this.name}, is not moving forward");

        _currentCarSpeed = 0;
    }

    private void SlowDown()
    {
        if(_currentCarSpeed == 0)
            return;
        
        if(debuggingMode)
            Debug.Log($"{this.name}, is not slowing down"); 
        
        ChangeCurrentSpeedGradually(frontDetectedObjectSpeed, 0.01f);
    }
    
    public void ChangeCurrentSpeedGradually(float targetSpeed, float smoothingFactor)
    {
        _currentCarSpeed = Mathf.Lerp(_currentCarSpeed, targetSpeed, smoothingFactor * Time.deltaTime);
    }
}

[System.Serializable]
public class RoadLane
{
    public Vector3 roadLanePosition;
    public RoadLaneType roadLaneType;
}

[System.Serializable]
public enum RoadLaneType
{
    CenterLane,
    LeftLane,
    RightLane,
}

[System.Serializable]
public enum CarState
{
    IsMovingForward,
    IsChangingLaneLeft,
    IsChangingLaneRight,
    IsNotMoving,
}