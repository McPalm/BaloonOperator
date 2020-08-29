using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColourFlash : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public ColourFlashPattern Pattern;

    float time = 999f;

    public Color baseColour { get; set; }

    void Start()
    {
        baseColour = Renderer.color;
    }

    public void PlayFlash()
    {
        time = 0f;
    }

    private void LateUpdate()
    {
        if (time < Pattern.duration)
        {
            time += Time.deltaTime;

            if (time < Pattern.duration)
                Renderer.color = Pattern.ColourFor(time);
            else
                Renderer.color = baseColour;
        }
    }

}
