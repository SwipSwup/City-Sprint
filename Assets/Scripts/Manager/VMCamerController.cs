using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMCamerController : MonoBehaviour
{
    private void Start() => SubscribeToEvents();
    private void OnDestroy() => UnSubscribeToEvents();
    private void SubscribeToEvents() => Player.OnGameOver += DisableVMCamera;
    private void UnSubscribeToEvents() => Player.OnGameOver -= DisableVMCamera;
    private void DisableVMCamera() => GetComponent<CinemachineVirtualCamera>().enabled = false;
}
