using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapModuleEditor : MonoBehaviour
{
    public Tilemap tilemap;
    public MapModuleSet MapModuleSet;

    const int gridSize = 13;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 5; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(i * gridSize, 0f), new Vector3(i * gridSize, 4 * gridSize));
            Gizmos.DrawLine(new Vector3(0f, i * gridSize), new Vector3(4 * gridSize, i * gridSize));
        }
        for(int i = 0; i < 16; i++)
        {
            if (MapModuleSet != null)
                PaintProps(i % 4, i / 4, MapModuleSet.MapModules[i]);
        }

        // SaveLoad();
    }

    void PaintProps(int x, int y, MapModuleSample sample)
    {
        Gizmos.color = Color.white;
        x *= gridSize;
        y *= gridSize;
        if (sample.OpenBothSides)
        {
            Gizmos.DrawLine(new Vector3(x + gridSize - .5f, y + 3f), new Vector3(x + gridSize, y + 3f));
            Gizmos.DrawLine(new Vector3(x, y + 3f), new Vector3(x + .5f, y + 3f));
        }
        if(sample.OpenBottom)
            Gizmos.DrawLine(new Vector3(x + gridSize / 2f, y), new Vector3(x + gridSize / 2f, y + .5f));
        if (sample.OpenTop)
            Gizmos.DrawLine(new Vector3(x + gridSize / 2f, y+gridSize), new Vector3(x + gridSize / 2f, y + gridSize - .5f));
        switch(sample.MapModuleFlag)
        {
            case MapModuleFlag.goal:
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(x, y), new Vector3(x + .5f, y + .5f));
                break;
            case MapModuleFlag.start:
                Gizmos.color = Color.green;
                Gizmos.DrawLine(new Vector3(x, y), new Vector3(x + .5f, y + .5f));
                break;
        }
    }

    public void Load()
    {
        for(int i = 0; i < 16; i++)
        {
            MapModuleSet.MapModules[i].PaintTo(tilemap, (i % 4) * gridSize, (i / 4) * gridSize, i % 4 == 0);
        }
    }

    public void Save()
    {
        for (int i = 0; i < 16; i++)
        {
            MapModuleSet.MapModules[i].CopyFrom(tilemap, (i % 4) * gridSize, (i / 4) * gridSize, i % 4 == 0);
            EditorUtility.SetDirty(MapModuleSet.MapModules[i]);
        }
    }

    [CustomEditor(typeof(MapModuleEditor))]
    public class MapModuleEditorEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Save"))
                ((MapModuleEditor)target).Save();
            if (GUILayout.Button("Load"))
                ((MapModuleEditor)target).Load();
                
        }
    }
}
