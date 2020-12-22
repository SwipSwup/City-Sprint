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

    private SerializedProperty endPointProp;
    private SerializedProperty startPointProp;
    private SerializedProperty isSpacerProp;
    private SerializedProperty tileSizeProp;
    private SerializedProperty tilePrefabProp;
    private SerializedProperty tilePartsProp;
    private SerializedProperty tilePartSizeProp;

    private bool showSettings = true;

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
        tilePartSizeProp = serializedObject.FindProperty("tilePartSize");

        tileParts = tile.tileParts;
        if(!tileParts.Contains(tile.transform.GetChild(0).GetChild(0).gameObject)) 
            tileParts.Add(tile.transform.GetChild(0).GetChild(0).gameObject);

        tile.UpdateCollider();
        tile.UpdateStartAndEndPoint();
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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(tilePartSizeProp, new GUIContent("Tile part size"));
            EditorGUILayout.PropertyField(isSpacerProp, new GUIContent("Is spacer"));
            EditorGUILayout.EndHorizontal();

           EditorGUILayout.IntSlider(tileSizeProp, 1, 60, new GUIContent("Tile size"));
            if (EditorGUI.EndChangeCheck()) tile.UpdateTileSize();
        }
    }   
}
