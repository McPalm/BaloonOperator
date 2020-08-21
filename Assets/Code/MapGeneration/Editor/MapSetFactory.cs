using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapSetFactory : EditorWindow
{
    string currentName;

    [MenuItem("Horselunky/New Module Set")]
    static void Init()
    {
        MapSetFactory window = CreateInstance<MapSetFactory>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Create a new Map Module Set", EditorStyles.wordWrappedLabel);
        currentName = EditorGUILayout.TextField("name", currentName);
        GUILayout.Space(70);

        if (GUILayout.Button("Create") && currentName.Length > 3) CreateNewSet(currentName);
        if (GUILayout.Button("Cancel")) this.Close();
    }

    static void CreateNewSet(string name)
    {

        var set = ScriptableObject.CreateInstance<MapModuleSet>();
        string path = $"Assets/Data/ModuleSets/{name}";

        AssetDatabase.CreateFolder("Assets/Data/ModuleSets", name);

        AssetDatabase.CreateAsset(set, path + ".asset");
        
        set.MapModules = new List<MapModuleSample>();
        for (int i = 0; i < 16; i++)
        {
            var sample = ScriptableObject.CreateInstance<MapModuleSample>();
            // sample.name = $"{name}_{i:00}.asset";
            AssetDatabase.CreateAsset(sample, path + $"/{name}_{i:00}.asset");
            set.MapModules.Add(sample);
        }
        
    }

}
