using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrackManagerEditor : Editor
{
    private TrackManager manager;

    private SerializedProperty starterTileProp;
    private GameObject starterTile;

    private bool showSettings = true;

    private void OnEnable()
    {
        manager = (TrackManager)target;

    }

    public override void OnInspectorGUI()
    {
        DrawSettings();

        DrawInfo();
    }

    private void Initialize()
    {
        starterTileProp = serializedObject.FindProperty("starterTile");
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
            manager.slowDownInterval = EditorGUILayout.FloatField(manager.slowDownInterval);
            manager.slowDownStepTime = EditorGUILayout.FloatField(manager.slowDownStepTime);
            EditorGUILayout.EndHorizontal();

        }
    }
}
