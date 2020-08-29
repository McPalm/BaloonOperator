using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapModuleList))]
public class MapModuleListInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            var list = (MapModuleList)target;
            list.Samples = new List<MapModuleSample>(GetAll());
            EditorUtility.SetDirty(list);
        }

        DrawDefaultInspector();
    }


    MapModuleSample[] GetAll()
    {

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(MapModuleSample).Name);  //FindAssets uses tags check documentation for more info
        MapModuleSample[] a = new MapModuleSample[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<MapModuleSample>(path);
        }
        return a;
    }
}
