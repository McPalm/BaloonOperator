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
        if (sample.OpenBottom)
            Gizmos.DrawLine(new Vector3(x + sizeX / 2f, y), new Vector3(x + sizeX / 2f, y + .5f));
        if (sample.OpenTop)
            Gizmos.DrawLine(new Vector3(x + sizeX / 2f, y + sizeY), new Vector3(x + sizeX / 2f, y + sizeY - .5f));
        
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

        public void OnSceneGUI()
        {
            var butt = ((MapModuleEditor)target).MapModuleSet.MapModules;
            for (int i = 0; i < butt.Count; i++)
            {
                    PaintLabels(i % 4, i / 4, butt[i]);
            }
        }
        void PaintLabels(int x, int y, MapModuleSample sample)
        {
            var pos = new Vector3((x + .5f) * sizeX, (y + .5f) * sizeY);
            var style = new GUIStyle();

            switch (sample.MapModuleFlag)
            {
                case MapModuleFlag.goal:
                    style.normal.textColor = Color.red;
                    Handles.Label(pos, "Goal", style);
                    break;
                case MapModuleFlag.start:
                    style.normal.textColor = Color.green;
                    Handles.Label(pos, "Start", style);
                    break;
                case MapModuleFlag.challenge:
                    style.normal.textColor = Color.yellow;
                    Handles.Label(pos, "Challenge", style);
                    break;
                case MapModuleFlag.monster:
                    style.normal.textColor = new Color(1f, .2f, .95f);
                    Handles.Label(pos, "Monster", style);
                    break;
            }
        }
    }

#endif
}
