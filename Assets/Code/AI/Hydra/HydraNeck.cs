using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraNeck : MonoBehaviour
{
    public Transform neckBase;
    public HydraAI Head;

    public Transform[] sections;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Head.isActiveAndEnabled)
        {
            for(int i = 0; i < sections.Length; i++)
            {
                var to = Vector3.Lerp(neckBase.position, Head.transform.position, ((i + 1) / (float)(sections.Length + 1)));
                sections[i].position = Vector3.Lerp(sections[i].position, to, .1f * (1f+i) / sections.Length);
            }
        }
        else
        {
            sections.ForEach(t => t.localPosition = new Vector3(0f, -5));
        }
    }
}
