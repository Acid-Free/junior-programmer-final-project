using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject plantObj;

    GameObject currentPlayer;
    List<GameObject> currentPlants = new List<GameObject>();

    float levelWidth = 6.0f;
    float levelHeight = 6.0f;
    float playerSpawnRadius = 3.0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = Random.insideUnitCircle * playerSpawnRadius;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        currentPlayer = Instantiate(playerObj, spawnPosition, playerObj.transform.rotation);
    }

    public void SpawnPlants()
    {
        for (int i = 0; i < GameManager.Instance.levelPlantCount; ++i)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-levelWidth, levelWidth), 0, Random.Range(-levelHeight, levelHeight));
            currentPlants.Add(Instantiate(plantObj, spawnPosition, plantObj.transform.rotation));
        }
    }

    public void DestroyAllUnits()
    {
        Destroy(currentPlayer);
        foreach(GameObject plant in currentPlants)
        {
            Destroy(plant);
        }
    }

    public void PlantDied()
    {
        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }
}
