using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapModuleList : ScriptableObject
{
    public List<MapModuleSample> Samples;

    public MapModuleSample GetByIndex(int index) => Samples[index];

    public int GetIndexFor(MapModuleSample sample) => Samples.IndexOf(sample);
        
         
}
