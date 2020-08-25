using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Colour Flash", menuName ="Colour Flash", order = 13)]
public class ColourFlashPattern : ScriptableObject
{
    public Gradient Gradient;
    public float duration = .5f;

    public Color ColourFor(float time) => Gradient.Evaluate(time / duration);
}
