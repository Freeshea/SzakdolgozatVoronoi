using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshDeformer))]
public class MeshDeformerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        MeshDeformer meshDeform = (MeshDeformer)target;

        if (GUILayout.Button("Mesh Deform"))
        {
            meshDeform.MeshDeform();
        }
    }
}