using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    public static Action<Tile> OnTileDelete;
    public static Action<Tile> OnBuildingDelete;
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Tile")
            OnTileDelete?.Invoke(other.GetComponent<Tile>());
        if(other.tag == "Building")
            OnBuildingDelete?.Invoke(other.GetComponent<Tile>());
        Destroy(other.gameObject);
    }
}
