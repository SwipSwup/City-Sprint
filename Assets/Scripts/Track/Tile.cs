using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public bool isSpacer;

    public int tileSize = 1;
    public GameObject tilePrefab;
    public List<GameObject> tileParts = new List<GameObject>();

    public float tileSpeed;
    private void UpdatedTileSpeed(float tileSpeed) => this.tileSpeed = tileSpeed;
    private void Awake() => TrackManager.OnUpdateTileSpeed += UpdatedTileSpeed;

    private void Move() => transform.Translate(new Vector3(tileSpeed * Time.deltaTime, 0f));
    private void Update() => Move();
}
