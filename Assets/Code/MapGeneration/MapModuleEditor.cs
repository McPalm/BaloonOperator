using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapModuleEditor : MonoBehaviour
{
    public Tilemap tilemap;
    public MapModuleSet MapModuleSet;

    const int sizeX = 17;
    const int sizeY = 13;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 5; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(i * sizeX, 0f), new Vector3(i * sizeX, 4 * sizeY));
            Gizmos.DrawLine(new Vector3(0f, i * sizeY), new Vector3(4 * sizeX, i * sizeY));
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
        x *= sizeX;
        y *= sizeY;
        if (sample.OpenBothSides)
        {
            Gizmos.DrawLine(new Vector3(x + sizeX - .5f, y + 3f), new Vector3(x + sizeX, y + 3f));
            Gizmos.DrawLine(new Vector3(x, y + 3f), new Vector3(x + .5f, y + 3f));
        }
        if(sample.OpenBottom)
            Gizmos.DrawLine(new Vector3(x + sizeX / 2f, y), new Vector3(x + sizeX / 2f, y + .5f));
        if (sample.OpenTop)
            Gizmos.DrawLine(new Vector3(x + sizeX / 2f, y+ sizeY), new Vector3(x + sizeX / 2f, y + sizeY - .5f));
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
            MapModuleSet.MapModules[i].PaintTo(tilemap, (i % 4) * sizeX, (i / 4) * sizeY, i % 4 == 0);
        }
    }

    public void Save()
    {
        for (int i = 0; i < 16; i++)
        {
            MapModuleSet.MapModules[i].CopyFrom(tilemap, (i % 4) * sizeX, (i / 4) * sizeY, i % 4 == 0);
#if UNITY_EDITOR
            EditorUtility.SetDirty(MapModuleSet.MapModules[i]);
#endif
        }
    }
#if UNITY_EDITOR
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
#endif
}
