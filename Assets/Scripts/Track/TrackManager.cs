﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public int maxTickCount;
    private int tickCounter;

    public GameObject[] tilePrefabs;
    public GameObject tileSpacer;
    public List<Tile> activeTiles = new List<Tile>();

    public int maxTiles = 10;
    public float tileSpeed = 5f;
    public float tileSpeedMultiplyer = 1.05f;
    public float maxTileSpeed = 25f;

    private void Start()
    {
        GameManager.OnTick += UpdateTileSpeed;
        TileDestroyer.OnTileDelete += TileDestroyed;

        InstaniateTrack();
    }

    private void Update()
    {


    }

    private void InstaniateTrack()
    {
        for (int i = 0; i < maxTiles; i++)
            SpawnTile(tilePrefabs[(int)UnityEngine.Random.Range(0f, tilePrefabs.Length)]);
        OnUpdateTileSpeed?.Invoke(tileSpeed);
    }

    public static Action<float> OnUpdateTileSpeed;
    public void UpdateTileSpeed()
    {
        if (++tickCounter % maxTickCount == 0)
        {
            tileSpeed = Mathf.Clamp(tileSpeed += tileSpeedMultiplyer, 0f, maxTileSpeed);
            OnUpdateTileSpeed?.Invoke(tileSpeed);
        }
    }

    private void TileDestroyed(Tile tile)
    {
        activeTiles.Remove(tile);
        if (!tile.isSpacer)
            SpawnTile(tilePrefabs[(int)UnityEngine.Random.Range(0f, tilePrefabs.Length)]);
    }

    private void SpawnTile(GameObject prefab)
    {
        GameObject newTile = Instantiate(tileSpacer, calculateNewTilePosition(tileSpacer), transform.rotation);
        newTile.GetComponent<Tile>().tileSpeed = tileSpeed;
        activeTiles.Add(newTile.GetComponent<Tile>());

        newTile = Instantiate(prefab, calculateNewTilePosition(prefab), transform.rotation);
        newTile.GetComponent<Tile>().tileSpeed = tileSpeed;
        activeTiles.Add(newTile.GetComponent<Tile>());
    }

    private Vector3 calculateNewTilePosition(GameObject prefab)
    {
        Vector3 lastBackPoint = activeTiles[activeTiles.Count - 1].backPoint.position;
        return new Vector3(lastBackPoint.x + (prefab.transform.position.x - prefab.GetComponent<Tile>().frontPoint.position.x), lastBackPoint.y, lastBackPoint.z);
    }
}
