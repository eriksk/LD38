using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropOffPoint))]
public class DropOffPointEditor : Editor
{   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Cheat Next ;)"))
            (target as DropOffPoint).CheatTeleportNextExpectedToLandingPad();
    }    
}