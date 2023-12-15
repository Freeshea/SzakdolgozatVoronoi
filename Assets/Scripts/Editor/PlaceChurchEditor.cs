using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceChurch))]
public class PlaceChurchEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlaceChurch placeObject = (PlaceChurch)target;

        if (GUILayout.Button("Place Churches"))
        {
            placeObject.PlaceChurchesOnMainCells();
        }
        if(GUILayout.Button("Place Trees"))
        {
            placeObject.PlaceTrees();
        }
    }
}
