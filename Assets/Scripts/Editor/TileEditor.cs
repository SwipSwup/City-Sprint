using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Collections.ObjectModel;

[CustomEditor(typeof(Tile))]
[ExecuteInEditMode]
public class TileEditor : Editor
{
    private Tile tile;
    private List<GameObject> tileParts;
    private GameObject tilePartPrefab;


    private BoxCollider collider;

    private SerializedProperty endPointProp;
    private SerializedProperty startPointProp;
    private SerializedProperty isSpacerProp;
    private SerializedProperty tileSizeProp;
    private SerializedProperty tilePrefabProp;
    private SerializedProperty tilePartsProp;

    private float tilePartSize;

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

        serializedObject.ApplyModifiedProperties();
    }

    private void Initialize() {
        tilePrefabProp = serializedObject.FindProperty("tilePrefab");
        tilePartsProp = serializedObject.FindProperty("tileParts");

        startPointProp = serializedObject.FindProperty("startPoint");
        endPointProp = serializedObject.FindProperty("endPoint");
        isSpacerProp = serializedObject.FindProperty("isSpacer");
        tileSizeProp = serializedObject.FindProperty("tileSize");

        tileParts = tile.tileParts;
        if(!tileParts.Contains(tile.transform.GetChild(0).GetChild(0).gameObject)) 
            tileParts.Add(tile.transform.GetChild(0).GetChild(0).gameObject);

        collider = tile.GetComponent<BoxCollider>();
        tilePartSize = 3f;

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
        EditorGUILayout.PropertyField(startPointProp, new GUIContent(string.Empty));
        EditorGUILayout.PropertyField(endPointProp, new GUIContent(string.Empty));
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

            EditorGUILayout.BeginHorizontal();
            tilePartSize = EditorGUILayout.FloatField("Part size", tilePartSize);
            EditorGUILayout.PropertyField(isSpacerProp, new GUIContent("Is spacer"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.IntSlider(tileSizeProp, 1, 30, new GUIContent("Tile size"));
            if (EditorGUI.EndChangeCheck()) UpdateTileSize();
        }
    }

    private void UpdateTileSize()
    {
        if (tile.tileSize == tileParts.Count) return;
        if (tile.tileSize > tileParts.Count) AddTileParts();
        if (tile.tileSize < tileParts.Count) RemoveTileParts();
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
        tile.startPoint.position = GetLastTilePart().transform.position + new Vector3(tilePartSize / 2f, 0f, 0f);
        tile.endPoint.position = GetFirstTilePart().transform.position + new Vector3(-tilePartSize / 2f, 0f, 0f);
    }

    private void AddTileParts()
    {
        while(tile.tileSize >= tileParts.Count)
        {
            GameObject newTilePart = Instantiate(tilePartPrefab, GetNewTilePartPosition(), tile.transform.rotation);
            tileParts.Add(newTilePart);
            newTilePart.transform.parent = tile.transform.GetChild(0);
        }
    }

    private void RemoveTileParts()
    {
        while (tile.tileSize < tileParts.Count)
        {
            GameObject lastTilePart = GetLastTilePart();
            DestroyImmediate(lastTilePart);
            tileParts.Remove(lastTilePart);
        }
    }
}
