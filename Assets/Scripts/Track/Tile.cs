using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isSpacer;

    public Transform frontPoint;
    public Transform backPoint;
    private void UpdatedTileSpeed(float tileSpeed) => this.tileSpeed = tileSpeed;
    public float tileSpeed;

    private void Awake()
    {
        TrackManager.OnUpdateTileSpeed += UpdatedTileSpeed;
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
