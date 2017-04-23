using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Robot))]
public class RobotEditor : Editor
{   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Kill"))
            (target as Robot).Kill();
    }    
}