using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoronoiGenerator))]
public class VoronoiGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VoronoiGenerator mapGen = target as VoronoiGenerator;

        if(DrawDefaultInspector())
        {
            if(mapGen.isAutoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
