﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGenerator map = target as MapGenerator;
        //map.GenerateMap();
    }
}
