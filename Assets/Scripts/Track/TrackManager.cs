using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public int maxTickCount;
    private int tickCounter;

    public List<Tile> activeTiles;
    public List<Tile> activeBuildings;
    public GameObject[] trackTilePrefabs;
    public GameObject[] menuTilePrefabs;
    public GameObject[] buildingPrefabs;
    public GameObject startTile;
    public GameObject tileSpacer;

    public int maxBuildings = 10;
    public Transform leftBuilding;
    public Transform rightBuilding;

    public int maxTiles = 10;
    public float maxTileSpeed = 25f;
    public float tileSpeed = 5f;
    public float tileSpeedMultiplyer = 1.05f;

    public float hitMultiplyer = 5f;
    public float hitStep = .5f;

    public float stopDownInterval = 1f;
    public float stopDownStepTime = .01f;

    private void Start()
    {
        SubscribeToEvents();
        Initialize();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        UnSubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        MainMenuUIHandler.OnPlay += StartTrack;
        TileDestroyer.OnBuildingDelete += ReactOnBuildingDestroyed;
        Player.OnGameOver += ReactOnGameOver;
        Player.OnObstacleCollision += ReactOnObstacleHit;
    }
    
    private void UnSubscribeToEvents()
    {
        MainMenuUIHandler.OnPlay -= StartTrack; 
        GameManager.OnTick -= ReactOnTickUpdate;
        TileDestroyer.OnBuildingDelete -= ReactOnBuildingDestroyed;
        TileDestroyer.OnTileDelete -= ReactOnTileDestroyedInRun;
        TileDestroyer.OnTileDelete -= ReactOnTileDestroyedInMenu;
        Player.OnGameOver -= ReactOnGameOver;
        Player.OnObstacleCollision -= ReactOnObstacleHit;
    }

    public void SetTileSpeedMultiplyer(float tileSpeedMultiplyer) => this.tileSpeedMultiplyer = tileSpeedMultiplyer;

    private void Initialize()
    {
        TileDestroyer.OnTileDelete += ReactOnTileDestroyedInMenu;
        AddstartTile();
        FillTrack(menuTilePrefabs, 3);
    }

    private void SpawnRandomMenuTile()
    {
        GameObject newTilePrefab = menuTilePrefabs[(int)UnityEngine.Random.Range(0f, menuTilePrefabs.Length)];

        SpawnTile(newTilePrefab, GetNewTrackTilePosition(newTilePrefab), transform.rotation);
    }

    private void FillTrack(GameObject[] tiles, int amount)
    {
        while (activeTiles.Count < amount)
        {
            GameObject tile = menuTilePrefabs[(int)UnityEngine.Random.Range(0f, menuTilePrefabs.Length)];
            SpawnTile(tile, GetNewTrackTilePosition(tile), transform.rotation);
        }
        UpdateTileSpeed(tileSpeed);
    }

    public static Action OnTrackStart;
    private void StartTrack()
    {
        GameManager.OnTick += ReactOnTickUpdate;
        TileDestroyer.OnTileDelete -= ReactOnTileDestroyedInMenu;
        TileDestroyer.OnTileDelete += ReactOnTileDestroyedInRun;
        StartCoroutine(InstaniateTrack());
        OnTrackStart?.Invoke();
    }

    private void AddstartTile()
    {
        activeTiles = new List<Tile>();
        activeTiles.Add(startTile.GetComponent<Tile>());
        startTile.GetComponent<Tile>().tileSpeed = tileSpeed;
    }

    private IEnumerator InstaniateTrack()
    {
        yield return null;
        while (activeTiles.Count < maxTiles)
            SpawnRandomTrackTile();
        UpdateTileSpeed(tileSpeed);
    }

    private GameObject GetRandomTile(GameObject[] tileList) => tileList[(int)UnityEngine.Random.Range(0f, tileList.Length)];

    private IEnumerator IncreaseTrackSpeed()
    {
        for (float i = hitMultiplyer; i > 0; i -= hitStep)
        {
            tileSpeed += hitStep;
            yield return new WaitForSeconds(.5f);
        }
    }

    private IEnumerator SmoothStopTrack()
    {
        while (tileSpeed > 0)
        {
            tileSpeed = Mathf.Clamp(tileSpeed - stopDownInterval, 0, maxTileSpeed);
            OnUpdateTileSpeed?.Invoke(tileSpeed);
            yield return new WaitForSeconds(stopDownStepTime);
        }
    }

    private void UpdateTileSpeed(float tileSpeed)
    {
        OnUpdateTileSpeed?.Invoke(tileSpeed);
    }

    public static Action<float> OnUpdateTileSpeed;
    public void ReactOnTickUpdate()
    {
        if (++tickCounter % maxTickCount == 0)
        {
            if (tileSpeed < maxTileSpeed)
                UpdateTileSpeed(tileSpeed = Mathf.Clamp(tileSpeed += tileSpeedMultiplyer, 0f, maxTileSpeed));
        }
    }

    private void ReactOnTileDestroyedInRun(Tile tile)
    {
        if (!tile.isSpacer)
            SpawnRandomTrackTile();

        activeTiles.Remove(tile);
    }

    private void ReactOnBuildingDestroyed(Tile tile)
    {
        activeBuildings.Remove(tile);

    }

    private void ReactOnTileDestroyedInMenu(Tile tile)
    {
        if (tile.isSpacer)
            SpawnRandomMenuTile();

        activeTiles.Remove(tile);
    }

    private void ReactOnObstacleHit()
    {
        StartCoroutine(IncreaseTrackSpeed());
    }

    private void ReactOnGameOver()
    {
        GameManager.OnTick -= ReactOnTickUpdate;
        StartCoroutine(SmoothStopTrack());
    }

    /// <summary>
    /// Calculates the new tracktile spawn position
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns> The new tracktile position</returns>
    private Vector3 GetNewTrackTilePosition(GameObject prefab)
    {
        Vector3 lastBackPoint = activeTiles[activeTiles.Count - 1].endPoint.position;
        return new Vector3(lastBackPoint.x + (prefab.transform.position.x - prefab.GetComponent<Tile>().startPoint.position.x), lastBackPoint.y, lastBackPoint.z);
    }

    /// <summary>
    /// Spawns a randomized Track tile from the TrackTilePrefab array
    /// </summary>
    private void SpawnRandomTrackTile()
    {
        GameObject newTilePrefab = GetRandomTile(trackTilePrefabs);

        SpawnTile(tileSpacer, GetNewTrackTilePosition(tileSpacer), transform.rotation);
        SpawnTile(newTilePrefab, GetNewTrackTilePosition(newTilePrefab), transform.rotation);
    }

   

    /// <summary>
    /// Spawns a tileprefab at given position and with the given rotation
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    private void SpawnTile(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newTile = Instantiate(prefab, position, rotation);
        newTile.GetComponent<Tile>().tileSpeed = tileSpeed;
        activeTiles.Add(newTile.GetComponent<Tile>());
    }

    private void SpawnRandomBuildingLeft()
    {
        GameObject newBuilding = Instantiate(buildingPrefabs[(int)UnityEngine.Random.Range(0f, buildingPrefabs.Length)], leftBuilding.position, leftBuilding.rotation);

        Tile tile = newBuilding.AddComponent<Tile>();
        tile.tileSpeed = tileSpeed;
        tile.isSpacer = true;
        activeBuildings.Add(tile);
    }

    private void SpawnRandomBuildingRight()
    {
        GameObject newBuilding = Instantiate(buildingPrefabs[(int)UnityEngine.Random.Range(0f, buildingPrefabs.Length)], rightBuilding.position, rightBuilding.rotation);

        Tile tile = newBuilding.AddComponent<Tile>();
        tile.tileSpeed = tileSpeed;
        tile.isSpacer = true;
        activeBuildings.Add(tile);
    }
}
