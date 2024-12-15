using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SpawnSnapScript))]
public class SpawnSnaps : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SpawnSnapScript script = (SpawnSnapScript)target;
        if(GUILayout.Button("Spawn Children.")) script.SpawnSnaps();
    }
}
