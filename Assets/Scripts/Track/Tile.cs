using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform frontPoint;
    public Transform backPoint;

    public bool isSpacer;

    [Range(1, 10)]
    public int tileSize = 1;
    public GameObject TilePrefab;
    public List<GameObject> tiles = new List<GameObject>();

    private void UpdatedTileSpeed(float tileSpeed) => this.tileSpeed = tileSpeed;
    public float tileSpeed;


    private void OnValidate()
    {
        UpdateTileSize();
    }

    private void Awake()
    {
        TrackManager.OnUpdateTileSpeed += UpdatedTileSpeed;
    }

    private void Update()
    {
        Move();
    }

    private void UpdateTileSize()
    {
        if (tileSize > tiles.Count)
            AddTiles();
        else
            RemoveTiles();
        UpdatePoints();
        UpdateCollider();
    }

    private void UpdatePoints()
    {
        frontPoint.position = new Vector3(tiles[tiles.Count - 1].transform.position.x + .75f, tiles[tiles.Count - 1].transform.position.y, tiles[tiles.Count - 1].transform.position.z);
        backPoint.position = new Vector3(tiles[0].transform.position.x - .75f, tiles[0].transform.position.y, tiles[0].transform.position.z);
    }

    private void UpdateCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.center = CalculateMiddlePoint();
        collider.size = new Vector3(1.5f * tiles.Count, 0f, 8f);
    }

    private Vector3 CalculateMiddlePoint()
    {
        return (tiles[tiles.Count - 1].transform.position + tiles[0].transform.position) / 2;
    }

    private void AddTiles()
    {
            for (int i = 0; i < tileSize - tiles.Count; i++)
            {
                GameObject tmp = Instantiate(
                    TilePrefab,
                    new Vector3((
                    tiles.Count < 1 ? transform.position.x : tiles[tiles.Count - 1].transform.position.x) + 1.5f,
                    transform.position.y,
                    transform.position.z),
                    transform.rotation
                    );
                tiles.Add(tmp);
                tmp.transform.parent = transform;
            }
    }

    private void RemoveTiles()
    {
        for (int i = 0; i < tiles.Count - tileSize; i++)
        {
            GameObject tmp = tiles[tiles.Count - 1];
            tiles.Remove(tmp);
            StartCoroutine(Destroy(tmp));
        }
    }

    IEnumerator Destroy(GameObject tile)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(tile);
    }

    private void Move()
    {
        transform.Translate(new Vector3(tileSpeed * Time.deltaTime, 0f));
    }
}
