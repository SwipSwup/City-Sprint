using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackManager))]
[ExecuteInEditMode]
public class TrackManagerEditor : Editor
{
    private TrackManager manager;

    private SerializedProperty startTileProp;
    private SerializedProperty tileSpacerProp;
    private SerializedProperty activeTilesProp;

    private GameObject _startTile;
    private GameObject _tileSpacer;

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
        startTileProp = serializedObject.FindProperty("startTile");
        tileSpacerProp = serializedObject.FindProperty("tileSpacer");
        activeTilesProp = serializedObject.FindProperty("activeTiles");
    }

    private void DrawInfo()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Current tile speed");
        GUILayout.Label("Current amount of tiles");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        manager.tileSpeed = EditorGUILayout.FloatField(manager.tileSpeed);
        EditorGUILayout.IntField(manager.activeTiles.Count);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Starting tile");
        GUILayout.Label("Spacer prefab");
        EditorGUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(startTileProp, new GUIContent(string.Empty));
        _startTile = startTileProp.objectReferenceValue as GameObject;
        EditorGUILayout.PropertyField(tileSpacerProp, new GUIContent(string.Empty));
        _tileSpacer = tileSpacerProp.objectReferenceValue as GameObject;
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(activeTilesProp, new GUIContent("Active Tiles"));

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
            manager.maxTiles = EditorGUILayout.IntField(manager.maxTiles);
            manager.maxTileSpeed = EditorGUILayout.FloatField(manager.maxTileSpeed);
            EditorGUILayout.EndHorizontal();

            manager.tileSpeedMultiplyer = EditorGUILayout.Slider("Tile speed multiplyer", manager.tileSpeedMultiplyer, 0f, 2f);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hit steps");
            GUILayout.Label("Hit multiplyer");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            manager.hitStep = EditorGUILayout.FloatField(manager.hitStep);
            manager.hitMultiplyer = EditorGUILayout.FloatField(manager.hitMultiplyer);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Stoping interval");
            GUILayout.Label("Stoping step time");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            manager.stopDownInterval = EditorGUILayout.FloatField(manager.stopDownInterval);
            manager.stopDownStepTime = EditorGUILayout.FloatField(manager.stopDownStepTime);
            EditorGUILayout.EndHorizontal();
        }
    }
}
