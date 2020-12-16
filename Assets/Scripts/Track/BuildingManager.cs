using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    [SerializeField] private int maxBuildings = 10;
    [SerializeField] private Transform leftBuildingSpawnLoc;
    [SerializeField] private Transform rightBuildingSpawnLoc;

    private float speed;

    void Start()
    {
        
    }

    private void SubscribeToEvents()
    {
    }

    private void UnsubscribeToEvents()
    {

    }


    private void SpawnRandomBuildingLeft()
    {
        GameObject newBuilding = Instantiate(buildingPrefabs[(int)UnityEngine.Random.Range(0f, buildingPrefabs.Length)], leftBuildingSpawnLoc.position, leftBuildingSpawnLoc.rotation);

        Tile tile = newBuilding.AddComponent<Tile>();
        tile.tileSpeed = speed;
        tile.isSpacer = true;
    }

    private void SpawnRandomBuildingRight()
    {
        GameObject newBuilding = Instantiate(buildingPrefabs[(int)UnityEngine.Random.Range(0f, buildingPrefabs.Length)], rightBuildingSpawnLoc.position, rightBuildingSpawnLoc.rotation);

        Tile tile = newBuilding.AddComponent<Tile>();
        tile.tileSpeed = speed;
        tile.isSpacer = true;
    }
}
