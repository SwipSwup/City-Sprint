﻿using UnityEngine;

public class ObstacelMixer : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacleVariants;
    [SerializeField] private bool rdmRotation = false;


    private void SpawnObstacle()
    {
        GameObject obstacle = obstacleVariants[(int)Random.Range(0f, obstacleVariants.Length)];
        GameObject obj = Instantiate(obstacle == null ? new GameObject() : obstacle, transform);
        if (rdmRotation && Random.Range(0, 2) == 0)
        {
            obj.transform.Rotate(0f, 180f, 0f);
        }
    }
    private void Awake() => SpawnObstacle();
}
