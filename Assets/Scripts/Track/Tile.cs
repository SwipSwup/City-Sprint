using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float tileSpeed = 1f;
    public Transform frontPoint;
    public Transform backPoint;


    private void Start()
    {
        Debug.Log(Vector3.Distance(transform.position, frontPoint.position));
    }
    private void Update()
    {
        transform.Translate(new Vector3(tileSpeed * Time.deltaTime, 0f));
    }
}
