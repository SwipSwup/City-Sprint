using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public List<Tile> activeTiles = new List<Tile>();
    public int maxTiles = 10;

    private void Start()
    {
        TileDestroyer.OnTileDelete += SpawnTile;
        for(int i = 0; i < maxTiles; i++)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        GameObject prefab = tilePrefabs[(int)Random.Range(0f, tilePrefabs.Length)];

        GameObject newTile = Instantiate(prefab, calculateNewTilePosition(prefab), transform.rotation);
        activeTiles.Add(newTile.GetComponent<Tile>());
    }

    private Vector3 calculateNewTilePosition(GameObject prefab)
    {
        Vector3 lastBackPoint = activeTiles[activeTiles.Count - 1].backPoint.position;
        return new Vector3(lastBackPoint.x + (prefab.transform.position.x - prefab.GetComponent<Tile>().frontPoint.position.x), lastBackPoint.y, lastBackPoint.z);
    }
}
