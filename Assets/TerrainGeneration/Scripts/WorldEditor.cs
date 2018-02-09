using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{

    public override void OnInspectorGUI()
    {
        World world = (World)target;

        if (DrawDefaultInspector() && world.displayAutoUpdate)
        {
            world.DisplayMap();
        }


        if (GUILayout.Button("Display"))
        {
            world.DisplayMap();
        }
    }
}