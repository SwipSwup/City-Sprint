using UnityEngine;

public class BalconyMan : MonoBehaviour
{
    [SerializeField] private int chance;
    private void OnEnable() => gameObject.SetActive(Random.Range(0, 100) < chance ? true : false);
}
