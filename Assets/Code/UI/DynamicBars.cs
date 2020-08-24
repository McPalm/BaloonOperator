using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DynamicBars : MonoBehaviour
{

    public bool showLessThanFull;
    public bool showOnChange;
    public float showDuration = 5f;
    float visibility = 1f;
    public float fadeDuration = .33f;

    Slider Slider;
    float lastValue = 0f;
    float visibleUntill = 0f;
    bool lastVisible;

    Image[] img;

    Color c0, c0b;
    Color c1, c1b;

    private void Start()
    {
        Slider = GetComponent<Slider>();
        img = GetComponentsInChildren<Image>();
        c0 = img[0].color;
        c1 = img[1].color;
        c0b = new Color(c0.r, c0.g, c0.b, 0f);
        c1b = new Color(c1.r, c1.g, c1.b, 0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Slider.value != lastValue && showOnChange)
        {
            visibleUntill = Time.timeSinceLevelLoad + showDuration;
        }
        lastValue = Slider.value;
        if (showLessThanFull && Slider.value < Slider.maxValue)
        {
            visibleUntill = Time.timeSinceLevelLoad + showDuration;
        }
        bool visible = visibleUntill > Time.timeSinceLevelLoad;
        if (visible)
            visibility = 1f;
        else if(visibility > 0f)
            visibility -= Time.deltaTime / fadeDuration;
        if(visibility < 0f)
        {
            img[0].enabled = false;
            img[1].enabled = false;
        }
        else
        {
            img[0].color = Color.Lerp(c0b, c0, visibility);
            img[1].color = Color.Lerp(c1b, c1, visibility);
            img[0].enabled = true;
            img[1].enabled = true;

        }
        
    }
}
