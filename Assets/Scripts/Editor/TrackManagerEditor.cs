using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackManager))]
[ExecuteInEditMode]
public class TrackManagerEditor : Editor
{
    private TrackManager manager;

    private SerializedProperty activeTilesProp;
    private SerializedProperty startTileProp;
    private SerializedProperty tileSpacerProp;
    private SerializedProperty tilePrefabsProp;
    private SerializedProperty menuTilePrefabsProp;

    private SerializedProperty maxTilesProp;
    private SerializedProperty maxTileSpeedProp;
    private SerializedProperty tileSpeedProp;
    private SerializedProperty tileSpeedMultiplyerProp;

    private SerializedProperty hitMultiplyerProp;
    private SerializedProperty hitStepProp;

    private SerializedProperty stopDownIntervalProp;
    private SerializedProperty stopDownStepTimeProp;
    
    private bool showSettings = true;

    private void OnEnable()
    {
        manager = (TrackManager)target;

        SetData();
    }

    public override void OnInspectorGUI()
    {
        DrawInfo();
        DrawSettings();

        serializedObject.ApplyModifiedProperties();
    }

    private void SetData()
    {
        activeTilesProp = serializedObject.FindProperty("activeTiles");
        startTileProp = serializedObject.FindProperty("startTile");
        tileSpacerProp = serializedObject.FindProperty("tileSpacer");
        tilePrefabsProp = serializedObject.FindProperty("trackTilePrefabs");
        menuTilePrefabsProp = serializedObject.FindProperty("menuTilePrefabs");

        maxTilesProp = serializedObject.FindProperty("maxTiles");
        maxTileSpeedProp = serializedObject.FindProperty("maxTileSpeed");
        tileSpeedProp = serializedObject.FindProperty("tileSpeed");
        tileSpeedMultiplyerProp = serializedObject.FindProperty("tileSpeedMultiplyer");

        hitMultiplyerProp = serializedObject.FindProperty("hitMultiplyer");
        hitStepProp = serializedObject.FindProperty("hitStep");

        stopDownIntervalProp = serializedObject.FindProperty("stopDownInterval");
        stopDownStepTimeProp = serializedObject.FindProperty("stopDownStepTime");
    }

    private void DrawInfo()
    {
       
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Current tile speed");
        GUILayout.Label("Current amount of tiles");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(tileSpeedProp, new GUIContent(string.Empty));
        EditorGUILayout.IntField(manager.activeTiles.Count);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Starting tile");
        GUILayout.Label("Spacer prefab");
        EditorGUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(startTileProp, new GUIContent(string.Empty));
        EditorGUILayout.PropertyField(tileSpacerProp, new GUIContent(string.Empty));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(tilePrefabsProp, new GUIContent("Tile prefabs"));
        EditorGUILayout.PropertyField(menuTilePrefabsProp, new GUIContent("Menu tile prefabs"));
        EditorGUILayout.PropertyField(activeTilesProp, new GUIContent("Active tiles"));

        GUILayout.Space(10);
    }

    private void DrawSettings()
    {
        showSettings = EditorGUILayout.Foldout(showSettings, "Settings", true);
        if (showSettings)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Max tiles");
            GUILayout.Label("Max tile speed");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(maxTilesProp, new GUIContent(string.Empty));
            EditorGUILayout.PropertyField(maxTileSpeedProp, new GUIContent(string.Empty));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Slider(tileSpeedMultiplyerProp, 1f, 2f, new GUIContent("Tile speed Multiplyer"));

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hit steps");
            GUILayout.Label("Hit multiplyer");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(hitStepProp, new GUIContent(string.Empty));
            EditorGUILayout.PropertyField(hitMultiplyerProp, new GUIContent(string.Empty));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Stoping interval");
            GUILayout.Label("Stoping step time");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(stopDownIntervalProp, new GUIContent(string.Empty));
            EditorGUILayout.PropertyField(stopDownStepTimeProp, new GUIContent(string.Empty));
            EditorGUILayout.EndHorizontal();
        }
    }
}
