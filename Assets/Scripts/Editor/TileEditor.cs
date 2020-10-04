using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    private Tile tile;
    private List<GameObject> tileParts;
    private SerializedProperty tilePartsProp;

    private SerializedProperty tilePrefabProp;
    private GameObject tilePartPrefab;

    private BoxCollider collider;

    private SerializedProperty startPointProp;
    private Transform _startPoint;
    private SerializedProperty endPointProp;
    private Transform _endPoint;

    private int tileSize;
    private float tilePartSize = 1.5f;

    private bool showSettings = true;
    private bool changeSize = true;

    private GameObject GetLastTilePart() => tileParts[tileParts.Count - 1];
    private GameObject GetFirstTilePart() => tileParts[0];
    private Vector3 GetNewTilePartPosition() => GetLastTilePart().transform.position + new Vector3(tilePartSize, 0, 0);

    private void OnEnable()
    {
        tile = (Tile)target;
        Initialize();
    }

    public override void OnInspectorGUI()
    {
        DrawTileInfo();
        EditorGUILayout.Space(5);
        DrawTileSettings();
    }

    private void Initialize() {
        tilePrefabProp = serializedObject.FindProperty("tilePrefab");
        tilePartsProp = serializedObject.FindProperty("tileParts");

        startPointProp = serializedObject.FindProperty("startPoint");
        endPointProp = serializedObject.FindProperty("endPoint");

        tileParts = tile.tileParts;
        if(!tileParts.Contains(tile.transform.GetChild(0).GetChild(0).gameObject)) 
            tileParts.Add(tile.transform.GetChild(0).GetChild(0).gameObject);

        collider = tile.GetComponent<BoxCollider>();
        tileSize = tileParts.Count;

        UpdateCollider();
        UpdateStartAndEndPoint();
    }

    private void DrawTileInfo()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("StartPoint");
        GUILayout.Label("EndPoint");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(startPointProp, new GUIContent(""));
        _startPoint = startPointProp.objectReferenceValue as Transform;
        EditorGUILayout.PropertyField(endPointProp, new GUIContent(""));
        _endPoint = endPointProp.objectReferenceValue as Transform;
        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(tilePartsProp, new GUIContent("Tile parts"));
    }

    private void DrawTileSettings()
    {
        showSettings = EditorGUILayout.Foldout(showSettings, "Tile settings", true);

        EditorGUI.BeginChangeCheck();
        if(showSettings)
        {
            EditorGUILayout.PropertyField(tilePrefabProp, new GUIContent("Tilepart prefab"));
            tilePartPrefab = tilePrefabProp.objectReferenceValue as GameObject;

            tileSize = EditorGUILayout.IntSlider("Tile size", tileSize, 1, 30);
            if (EditorGUI.EndChangeCheck()) UpdateTileSize();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateTileSize()
    {
        if (tileSize == tileParts.Count) return;
        if (tileSize > tileParts.Count) AddTileParts();
        if (tileSize < tileParts.Count) RemoveTileParts();
        UpdateCollider();
        UpdateStartAndEndPoint();
    }

    private Vector3 CalculateMidPoint() => 
        (tileParts[tileParts.Count - 1].transform.localPosition + tileParts[0].transform.localPosition) / 2;
    private void UpdateCollider()
    {
        collider.center = CalculateMidPoint();
        collider.size = new Vector3(tilePartSize * tileParts.Count, 0f, 8f);
    }

    private void UpdateStartAndEndPoint()
    {
        _startPoint.position = GetLastTilePart().transform.position + new Vector3(tilePartSize / 2f, 0f, 0f);
        _endPoint.position = GetFirstTilePart().transform.position + new Vector3(-tilePartSize / 2f, 0f, 0f);
    }

    private void AddTileParts()
    {
        while(tileSize > tileParts.Count)
        {
            GameObject newTilePart = Instantiate(tilePartPrefab, GetNewTilePartPosition(), tile.transform.rotation);
            newTilePart.transform.parent = tile.transform.GetChild(0);
            tileParts.Add(newTilePart);
        }
    }

    private void RemoveTileParts()
    {
        while(tileSize < tileParts.Count)
        {
            GameObject lastTilePart = GetLastTilePart();
            tileParts.Remove(lastTilePart);
            DestroyImmediate(lastTilePart);
        }
    }
}
