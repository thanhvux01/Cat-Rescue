using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManagement : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private int  worldSeed;
    [SerializeField] private bool isGenerateWithWorldSeed;
    [SerializeField] private int  difficultyLevel;
    [SerializeField] private float  difficultyMultiplier;
    
    [Header("Cat Settings")]
    [SerializeField] private Cat catPrefabTemplate;
    [SerializeField] private float distanceObstacle;
    [SerializeField] private int numberOfCatsToSpawn; 
    [SerializeField] private Vector2 minMaxXCatSpawn; 
    [SerializeField] private Vector2 minMaxZCatSpawn; 

    [Header("Obstacle Settings")]
    [SerializeField] private GameObject[] obstaclePrefabs; 
    [SerializeField] private Vector2 minMaxXObstacleSpawn; 
    [SerializeField] private Vector2 minMaxZObstacleSpawn; 

    [SerializeField] private float startZPosition = 50f; 
    [SerializeField] private float endZPosition = 120f;
    
    private int seed;
    
    private List<Cat> spawnedCats = new List<Cat>();
    
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    public List<Cat> SpawnedCats => spawnedCats;
    
    public List<GameObject> SpawnedObstacles => spawnedObstacles;
    
    public float StartZPosition => startZPosition; 
    public float EndZPosition => endZPosition;
    public float MaxZCatSpawn => minMaxZCatSpawn.y;
    public int Seed => seed;
    public int DifficultyLevel => difficultyLevel;

    public Transform spawnPoint;
    
    public Transform endPoint;
    
    public Tsunami tsunami;

    [ContextMenu("Generate Map")]
    public void Generate()
    {
        ClearMap();
        GenerateSeed();
        FindObjectsInScene();
    
        float currentSpawnZ = startZPosition;
        float zMultiplier = Mathf.Clamp(1 - difficultyMultiplier * difficultyLevel, 0.2f, 1.0f); 
        tsunami.moveSpeed += difficultyMultiplier* difficultyLevel;
        
        while (currentSpawnZ < endZPosition)
        {
            float zOffset = Random.Range(minMaxZObstacleSpawn.x, minMaxZObstacleSpawn.y) * zMultiplier;
            currentSpawnZ += zOffset;
            float obstacleXPosition = Random.Range(minMaxXObstacleSpawn.x, minMaxXObstacleSpawn.y);
            GameObject selectedObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            GameObject newObstacle = Instantiate(selectedObstaclePrefab, new Vector3(obstacleXPosition, 3.5f, currentSpawnZ), Quaternion.identity);
            spawnedObstacles.Add(newObstacle);
        }
        
        SpawnCats();
    }
    
    private void SpawnCats()
    {
        int spawnedCount = 0;
        while (spawnedCount < numberOfCatsToSpawn)
        {
            float randomZ = Random.Range(startZPosition, endZPosition);
            float randomX = Random.Range(minMaxXCatSpawn.x, minMaxXCatSpawn.y);
            
            bool isValidPosition = true;
            foreach (var obstacle in spawnedObstacles)
            {
                if (Vector3.Distance(new Vector3(randomX, 0, randomZ), obstacle.transform.position) < distanceObstacle)
                {
                    isValidPosition = false;
                    break;
                }
            }
            if (isValidPosition)
            {
                Cat newCat = Instantiate(catPrefabTemplate, new Vector3(randomX, 3.5f, randomZ), Quaternion.identity);
                spawnedCats.Add(newCat);
                spawnedCount++;
            }
        }
    }

    public void Generate10Map()
    {
        difficultyLevel = 0;
        
        for (int i = 0; i < 10; i++)
        {
            difficultyLevel += ;
            Generate();
        }
    }

    public void SetRandomWorldSeed(bool value)
    {
        isGenerateWithWorldSeed = value;
    }
    
    public void SetDifficultyLevel(int level)
    {
        difficultyLevel = level;
    }
    private void FindObjectsInScene()
    {
        //MUST DO Ensure that the spawn point and end point are tagged in the scene
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        endPoint   = GameObject.FindGameObjectWithTag("EndPoint").transform;
        tsunami    = GameObject.FindGameObjectWithTag("Tsunami").GetComponent<Tsunami>();
    }
    private void GenerateSeed()
    {
        if(isGenerateWithWorldSeed)
        {
            seed = worldSeed;
            Random.InitState(seed);
            Debug.Log("Seed of current map is"+seed);
            return;
        }
        seed = Random.Range(1, 100000);
        Random.InitState(seed);
        Debug.Log("Seed of current map is"+seed);
    }

    [ContextMenu("Clear Map")]
    public void ClearMap()
    {
        spawnedCats.ForEach(Destroy);
        spawnedObstacles.ForEach(Destroy);
        spawnedCats.Clear();
        spawnedObstacles.Clear();
    }
}