using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponsList))]
public class WeaponsListInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            var list = (WeaponsList)target;
            list.WeaponProperties = GetAll();
            EditorUtility.SetDirty(list);
        }

        DrawDefaultInspector();
    }


    WeaponProperties[] GetAll()
    {

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(WeaponProperties).Name);  //FindAssets uses tags check documentation for more info
        WeaponProperties[] a = new WeaponProperties[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<WeaponProperties>(path);
        }
        return a;
    }
}
