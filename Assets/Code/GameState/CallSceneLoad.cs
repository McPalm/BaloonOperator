using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSceneLoad : MonoBehaviour
{
    [Scene]
    public string SceneToLoad;


    public float autoFireInSeconds = 0f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(autoFireInSeconds > 0f)
        {
            yield return null;
            yield return new WaitForSeconds(autoFireInSeconds);
        }
    }

    public void LoadNextScene()
    {
        var loader = FindObjectOfType<SceneLoader>();
        loader.LoadScene(SceneToLoad);
    }
}
