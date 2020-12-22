using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public bool isSpacer;

    public int tileSize = 1;
    public float tilePartSize = 3f;
    public GameObject tilePrefab;
    public List<GameObject> tileParts = new List<GameObject>();
    private GameObject tilePartPrefab;

    private new BoxCollider collider;

    public float tileSpeed;
    private void UpdatedTileSpeed(float tileSpeed) => this.tileSpeed = tileSpeed;
    private void Start()
    {
        TrackManager.OnUpdateTileSpeed += UpdatedTileSpeed;
        collider = GetComponent<BoxCollider>();
    }

    private void Move() => transform.Translate(new Vector3(tileSpeed * Time.deltaTime, 0f));
    private void Update() => Move();

    private GameObject GetLastTilePart() => tileParts[tileParts.Count - 1];
    private GameObject GetFirstTilePart() => tileParts[0];
    private Vector3 GetNewTilePartPosition() => GetLastTilePart().transform.position + new Vector3(tilePartSize, 0, 0);

    public void UpdateTileSize()
    {
        if (tileSize == tileParts.Count) return;
        if (tileSize > tileParts.Count) AddTileParts();
        else if (tileSize < tileParts.Count) RemoveTileParts();
        UpdateCollider();
        UpdateStartAndEndPoint();
    }

    private Vector3 CalculateMidPoint() =>
        (tileParts[tileParts.Count - 1].transform.localPosition + tileParts[0].transform.localPosition) / 2;
    public void UpdateCollider()
    {
        collider.center = CalculateMidPoint();
        collider.size = new Vector3(tilePartSize * tileParts.Count, 0f, 8f);
    }

    public void UpdateStartAndEndPoint()
    {
        startPoint.position = GetLastTilePart().transform.position + new Vector3(tilePartSize / 2f, 0f, 0f);
        endPoint.position = GetFirstTilePart().transform.position + new Vector3(-tilePartSize / 2f, 0f, 0f);
    }

    public void AddTileParts()
    {
        while (tileSize > tileParts.Count)
        {
            GameObject newTilePart = Instantiate(tilePrefab, GetNewTilePartPosition(), transform.rotation);
            tileParts.Add(newTilePart);
            newTilePart.transform.parent = transform.GetChild(0);
        }
    }

    public void RemoveTileParts()
    {
        while (tileSize < tileParts.Count)
        {
            GameObject lastTilePart = GetLastTilePart();
            DestroyImmediate(lastTilePart);
            tileParts.Remove(lastTilePart);
        }
    }
}
