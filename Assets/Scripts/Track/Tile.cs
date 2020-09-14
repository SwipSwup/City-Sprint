using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public Transform frontPoint;
    public Transform backPoint;

    public bool isSpacer;

    [Range(1, 10)]
    public int tileSize = 1;

    private void UpdatedTileSpeed(float tileSpeed) => this.tileSpeed = tileSpeed;
    public float tileSpeed;

    private void Awake()
    {
        TrackManager.OnUpdateTileSpeed += UpdatedTileSpeed;
    }

    private void UpdateTileSize()
    {

    }

    private void AddTile()
    {

    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        transform.Translate(new Vector3(tileSpeed * Time.deltaTime, 0f));
    }
}
