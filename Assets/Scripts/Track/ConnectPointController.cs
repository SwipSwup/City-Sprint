using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConnectPointController : MonoBehaviour
{
    [SerializeField] private int pointSpacing;
    private Transform rootpointTransform;
    private Transform pointLeft;
    private Transform pointRight;

    private void OnEnable()
    {
        GenerateNeighbours();
    }

    private void Update()
    {
        SetSpacing();
    }

    private void GenerateNeighbours()
    {
        pointLeft = new GameObject("Point_left").transform;
        pointLeft.parent = rootpointTransform;
        pointRight = new GameObject("Point_right").transform;
        pointRight.parent = rootpointTransform;

        SetSpacing();
    }

    private void SetSpacing()
    {
        pointLeft.position *= -pointSpacing;
        pointRight.position *= pointSpacing;
    }
}
