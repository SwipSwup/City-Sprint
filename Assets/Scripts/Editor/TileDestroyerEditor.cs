using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileDestroyer))]
public class TileDestroyerEditor : Editor
{

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    public static void OnDrawSceneGizmo(TileDestroyer destroyer, GizmoType gizmotype)
    {
        BoxCollider collider = destroyer.GetComponent<BoxCollider>();

        Gizmos.color = (GizmoType.Selected & gizmotype) != 0 ? Color.cyan : Color.cyan * .5f;
        Gizmos.DrawWireCube(destroyer.transform.position, collider.size);
        Gizmos.color = Color.green;
    }
    private void OnInspectorGUI()
    {
    }


}
