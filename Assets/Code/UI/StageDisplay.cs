using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    

    // Update is called once per frame
    void Update()
    {
        text.text = $"Stage {GameManager.StageDifficulty + 1}";
    }
}
