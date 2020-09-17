using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] private float spinSpeed;

    [SerializeField] private float bounceHeight;
    [SerializeField] private float bounceDuration;
    [SerializeField] private LeanTweenType easeType;

    private void Awake() => LeanTween.moveY(gameObject, transform.position.y + bounceHeight, bounceDuration).setLoopPingPong().setEase(easeType);

    private void Update() => Spin();

    private void Spin()
    {
        Vector3 angle = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angle.x, angle.y + spinSpeed, angle.z);
    }
}
