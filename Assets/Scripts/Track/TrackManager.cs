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
    public GameObject[] trackTilePrefabs;
    public GameObject[] menuTilePrefabs;
    public GameObject startTile;
    public GameObject tileSpacer;

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
        Player.OnGameOver += HandleGameOver;
        Player.OnObstacleCollision += HandleObstacleHit;
    }
    
    private void UnSubscribeToEvents()
    {
        MainMenuUIHandler.OnPlay -= StartTrack; 
        GameManager.OnTick -= HandleTickUpdate;
        TileDestroyer.OnTileDelete -= HandleTileDestroyedOnRun;
        TileDestroyer.OnTileDelete -= HandleTileDestroyedOnMenu;
        Player.OnGameOver -= HandleGameOver;
        Player.OnObstacleCollision -= HandleObstacleHit;
    }

    public void SetTileSpeedMultiplyer(float tileSpeedMultiplyer) => this.tileSpeedMultiplyer = tileSpeedMultiplyer;

    private void Initialize()
    {
        TileDestroyer.OnTileDelete += HandleTileDestroyedOnMenu;
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
        GameManager.OnTick += HandleTickUpdate;
        TileDestroyer.OnTileDelete -= HandleTileDestroyedOnMenu;
        TileDestroyer.OnTileDelete += HandleTileDestroyedOnRun;
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

    private IEnumerator SmoothSpeedUpTileSpeed()
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
    public void HandleTickUpdate()
    {
        if (++tickCounter % maxTickCount == 0)
        {
            if (tileSpeed < maxTileSpeed)
                UpdateTileSpeed(tileSpeed = Mathf.Clamp(tileSpeed += tileSpeedMultiplyer, 0f, maxTileSpeed));
        }
    }

    private void HandleTileDestroyedOnRun(Tile tile)
    {
        if (!tile.isSpacer)
            SpawnRandomTrackTile();

        activeTiles.Remove(tile);
    }

    private void HandleTileDestroyedOnMenu(Tile tile)
    {
        if (tile.isSpacer)
            SpawnRandomMenuTile();

        activeTiles.Remove(tile);
    }

    private void HandleObstacleHit()
    {
        StartCoroutine(SmoothSpeedUpTileSpeed());
    }

    private void HandleGameOver()
    {
        GameManager.OnTick -= HandleTickUpdate;
        StartCoroutine(SmoothStopTrack());
    }

    private Vector3 GetNewTrackTilePosition(GameObject prefab)
    {
        Vector3 lastBackPoint = activeTiles[activeTiles.Count - 1].endPoint.position;
        return new Vector3(lastBackPoint.x + (prefab.transform.position.x - prefab.GetComponent<Tile>().startPoint.position.x), lastBackPoint.y, lastBackPoint.z);
    }

    private void SpawnRandomTrackTile()
    {
        GameObject newTilePrefab = GetRandomTile(trackTilePrefabs);

        SpawnTile(tileSpacer, GetNewTrackTilePosition(tileSpacer), transform.rotation);
        SpawnTile(newTilePrefab, GetNewTrackTilePosition(newTilePrefab), transform.rotation);
    }

    private void SpawnTile(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newTile = Instantiate(prefab, position, rotation);
        newTile.GetComponent<Tile>().tileSpeed = tileSpeed;
        activeTiles.Add(newTile.GetComponent<Tile>());
    }
}
