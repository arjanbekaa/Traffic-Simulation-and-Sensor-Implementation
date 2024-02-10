using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject roadPrefab; // Prefab of the road segment to spawn
    public int initialRoadCount = 5; // Number of road segments to spawn initially
    public float spawnDistance = 200f; // Distance between each road segment
    public float despawnDistance = 50f; // Distance behind the main car to despawn road segments
    public Transform mainCar; 

    private List<GameObject> roads = new List<GameObject>();
    private Queue<GameObject> inactiveRoads = new Queue<GameObject>(); 
    private float lastSpawnedZ; 

    private void Start()
    {
        lastSpawnedZ = mainCar.position.z;
        
        for (int i = 0; i < initialRoadCount; i++)
        {
            SpawnRoad();
        }
    }

    private void Update()
    {
        foreach (GameObject road in roads)
        {
            if (road.activeSelf && road.transform.position.z < mainCar.position.z - despawnDistance)
            {
                road.SetActive(false);
                inactiveRoads.Enqueue(road);
                
                SpawnRoad();
            }
        }
    }

    private void SpawnRoad()
    {
        GameObject road;

        if (inactiveRoads.Count > 0)
        {
            road = inactiveRoads.Dequeue();
            road.transform.position = new Vector3(0f, 0f, lastSpawnedZ + spawnDistance);
            road.SetActive(true);
        }
        else
        {
            road = Instantiate(roadPrefab, new Vector3(0f, 0f, lastSpawnedZ + spawnDistance), Quaternion.identity, this.transform);
            roads.Add(road);
        }

        lastSpawnedZ += spawnDistance;
    }
}
