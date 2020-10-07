using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MySQLController))]
public class MySQLControllerEditor : Editor
{

    string email;
    string displayName;
    int score;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Email");
        GUILayout.Label("Display name");
        GUILayout.Label("Score");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        email = EditorGUILayout.TextField(email);
        displayName = EditorGUILayout.TextField(displayName);
        score = EditorGUILayout.IntField(score);
        EditorGUILayout.EndHorizontal();
        
        if(GUILayout.Button("Submit score"))
        {
            MySQLController msc = (MySQLController)target;
            msc.postScoreV2(email, displayName, score);
        }
    }
}
