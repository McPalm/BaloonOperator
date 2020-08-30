using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    public Slider Slider;

    public IEnumerator Start()
    {
        while (true)
        {
            var gm = FindObjectOfType<GameManager>();
            if(gm?.Player != null)
            {
                gm.Player.GetComponent<Health>().OnChangeHealth.AddListener(Slider.SetValueWithoutNotify);
                break;
            }
            yield return null;
        }
    }
}
