using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapModuleEditor : MonoBehaviour
{
    public bool save;
    public bool load;
    public MapModuleSample currentSample;
    public Tilemap tilemap;


    private void OnDrawGizmosSelected()
    {
        if(save)
        {
            currentSample.CopyFrom(tilemap, 0, 0);
            save = false;
#if UNITY_EDITOR
            EditorUtility.SetDirty(currentSample);
#endif
        }
        else if(load)
        {
            currentSample.PaintTo(tilemap, 0, 0);
            load = false;
        }
    }
}
