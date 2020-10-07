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
    public GameObject[] tilePrefabs;
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
        DefaultSubscriptions();
        InstaniateTrack();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void DefaultSubscriptions()
    {
        GameManager.OnTick += HandleTickUpdate;
        TileDestroyer.OnTileDelete += HandleTileDestroyed;
        Player.OnGameOver += HandleGameOver;
        Player.OnObstacleCollision += HandleObstacleHit;
    }

    public void SetTileSpeedMultiplyer(float tileSpeedMultiplyer)
    {
        this.tileSpeedMultiplyer = tileSpeedMultiplyer;
    }

    private void InstaniateTrack()
    {
        activeTiles = new List<Tile>();
        activeTiles.Add(startTile.GetComponent<Tile>());

        while (activeTiles.Count < maxTiles)
            SpawnRandomTrackTile();

        UpdateTileSpeed(tileSpeed);
    }

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

    private void HandleTileDestroyed(Tile tile)
    {
        activeTiles.Remove(tile);

        if (!tile.isSpacer)
            SpawnRandomTrackTile();
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
        Debug.Log(activeTiles.Count);
        Vector3 lastBackPoint = activeTiles[activeTiles.Count - 1].endPoint.position;
        return new Vector3(lastBackPoint.x + (prefab.transform.position.x - prefab.GetComponent<Tile>().startPoint.position.x), lastBackPoint.y, lastBackPoint.z);
    }

    private void SpawnRandomTrackTile()
    {
        GameObject newTilePrefab = tilePrefabs[(int)UnityEngine.Random.Range(0f, tilePrefabs.Length)];

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
