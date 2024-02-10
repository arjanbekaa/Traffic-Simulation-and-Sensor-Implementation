using System.Collections.Generic;
using UnityEngine;

public class RoadTrafficManager : MonoBehaviour
{
    public SmartCarTrafficMovement carPrefab; // Prefab of the car to spawn
    public int initialCarCount = 10; // Number of cars to spawn initially
    public float spawnDistance = 100f; // Distance ahead of the main car to spawn new cars
    public float despawnDistance = 200f; // Distance behind the main car to despawn cars
    public Transform mainCar; // Transform of the main car
    public float minSpeed = 5f; // Minimum speed for spawned cars
    public float maxSpeed = 15f; // Maximum speed for spawned cars

    private List<SmartCarTrafficMovement> cars = new List<SmartCarTrafficMovement>(); // List to hold active cars
    private Queue<SmartCarTrafficMovement> inactiveCars = new Queue<SmartCarTrafficMovement>(); // Queue to hold inactive cars for object pooling

    private void Start()
    {
        // Instantiate initial cars
        for (int i = 0; i < initialCarCount; i++)
        {
            SpawnCar();
        }
    }

    private void Update()
    {
        // Check if any active cars need to be despawned
        foreach (SmartCarTrafficMovement car in cars)
        {
            if (car.gameObject.activeSelf && car.transform.position.z < mainCar.position.z - despawnDistance)
            {
                car.gameObject.SetActive(false);
                inactiveCars.Enqueue(car);
                
                SpawnCar();
            }
        }
    }

    private void SpawnCar()
    {
        SmartCarTrafficMovement car;

        // Use object pooling if there are inactive cars available
        if (inactiveCars.Count > 0)
        {
            car = inactiveCars.Dequeue();
            car.transform.position = GetRandomSpawnPosition(mainCar.position);
            car.gameObject.SetActive(true);
        }
        else
        {
            car = Instantiate(carPrefab, GetRandomSpawnPosition(mainCar.position), Quaternion.identity, this.transform);
            cars.Add(car);
        }

        // Set random speed for the spawned car
        car.ChangeCurrentSpeed(Random.Range(minSpeed, maxSpeed));
    }

    private Vector3 GetRandomSpawnPosition(Vector3 spawnPosition)
    {
        spawnPosition.z += spawnDistance;
        float[] lanePositions = { -3.75f, 0f, 3.75f };
        float randomXOffset = lanePositions[Random.Range(0, lanePositions.Length)];

        foreach (SmartCarTrafficMovement car in cars)
        {
            if (Mathf.Abs(car.transform.position.z - spawnPosition.z) < 20)
            {
                if (Mathf.Approximately(car.transform.position.x, randomXOffset) || Random.Range(0,10) != 0)
                    spawnPosition.z += spawnDistance;
            }
        }
        
        return new Vector3(randomXOffset, spawnPosition.y, spawnPosition.z);
    }

}
