using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacelMixer : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacleVariants;

    private void SpawnObstacle() => Instantiate(obstacleVariants[(int)Random.Range(0f, obstacleVariants.Length)], transform);
    private void Awake() => SpawnObstacle();
}
