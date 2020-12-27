using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private Material[] Materials;

    private void OnEnable() => GetComponent<MeshRenderer>().material = Materials[(int)UnityEngine.Random.Range(0f, Materials.Length)];

}
