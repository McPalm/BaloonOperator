﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GeneratorSet : ScriptableObject
{
    public MapModuleSet[] Standard;
    public MapModuleSet[] Bottom;
    public MapModuleSet[] Top;
    public MapModuleSet[] Rare;
}
