using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    public static Action OnTileDelete;
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Tile")
            OnTileDelete?.Invoke();
        Destroy(other.gameObject);
    }
}
