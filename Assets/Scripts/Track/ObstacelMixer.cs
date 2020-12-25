using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacelMixer : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacleVariants;

    private void SpawnObstacle()
    {
        GameObject obstacle = obstacleVariants[(int)Random.Range(0f, obstacleVariants.Length)];
        Instantiate(obstacle == null ? new GameObject() : obstacle, transform);
    }
    private void Awake() => SpawnObstacle();
}
