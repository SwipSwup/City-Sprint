using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    private void OnEnable() => transform.rotation.Set(transform.rotation.x, Random.Range(0, 360), transform.rotation.z, transform.rotation.w);
}
