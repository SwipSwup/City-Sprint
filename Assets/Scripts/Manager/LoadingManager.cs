using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{

    [SerializeField] private Transform car;
    [SerializeField] private TextMeshProUGUI loadingTxt;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private bool load = true;

    [SerializeField] private float startPosition;
    [SerializeField] private float wobbleStrength;
    [SerializeField] private float wobbleSpeed;
    [SerializeField] private float rotationSpeed;

    private float x;

    private void Start()
    {
        if(load)
            StartCoroutine(LoadAsynchronously());
    }

    private void Update()
    {
        StartCoroutine(Hover());
    }

    private IEnumerator LoadAsynchronously()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            loadingTxt.text = "loading... " + (int)progress * 100 + "%";
            loadingSlider.value = progress;
            yield return null;
        }
    }

    // wobbles acade machine
    private IEnumerator Hover()
    {
        car.Rotate(0f, 0f, rotationSpeed, Space.Self);

        car.position = new Vector3(
            car.position.x,
            startPosition + Mathf.Sin(x * Mathf.PI) * wobbleStrength,
            car.position.z
            );
        x += wobbleSpeed;
        yield return new WaitForSeconds(10f);
    }

}
