using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;

    private void Start()
    {
        TrackManager.OnTrackStart += blendToV1;
    }

    private void OnDestroy()
    {
        TrackManager.OnTrackStart -= blendToV1;
    }

    private void blendToV1()
    {
        Debug.Log("blend");
        vcam1.enabled = true;
    }

    private void blendToV2()
    {
        vcam1.enabled = false;
    }
}
