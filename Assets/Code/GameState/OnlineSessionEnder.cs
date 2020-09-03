using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineSessionEnder : MonoBehaviour
{
    public float EndSessionInSeconds = 0f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(EndSessionInSeconds > 0f)
        {
            yield return null;
            yield return new WaitForSeconds(EndSessionInSeconds);
            EndSession();
        }
    }

   public void EndSession()
    {
        MyNetworkManager.singleton.StopClient();
        if(MyNetworkManager.isServer)
            MyNetworkManager.singleton.StopHost();
    }
}
