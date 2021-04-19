using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTilt : MonoBehaviour
{

    PlatformingCharacter player;
    InputToken InputToken;
    public Transform Camera;
    public Vector3 Offset;



    // Start is called before the first frame update
    IEnumerator Start()
    {
        Vector3 baseOffset = Camera.transform.localPosition;
        CameraFollow cameraFollow = null;
        while (cameraFollow == null)
        {
            cameraFollow = GetComponent<CameraFollow>();
            yield return null;
        }
        GameObject target = null;
        while (target == null)
        {
            if (cameraFollow.Follow.Length > 0)
                target = cameraFollow.Follow[0].gameObject;
            yield return null;
        }
        player = target.GetComponent<PlatformingCharacter>();
        while (InputToken == null)
        {
            InputToken = player.InputToken;
            yield return null;
        }

    neutral:
        cameraFollow.ShiftFocus(baseOffset, .2f);
        for (; ; )
        {
            if (player.Grounded && InputToken.Direction.y > .5f && Mathf.Abs(InputToken.Direction.x) < .25f)
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return new WaitForSeconds(.1f);
                    if (InputToken.Direction.y < .5f)
                        goto skip;
                }
                goto lookup;
            }
            if (player.Grounded && InputToken.Direction.y < -.5f && Mathf.Abs(InputToken.Direction.x) < .25f)
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return new WaitForSeconds(.1f);
                    if (InputToken.Direction.y > -.5f)
                        goto skip;
                }
                goto lookdown;
            }
        skip:
            yield return null;
        }
    lookup:
        cameraFollow.ShiftFocus(baseOffset + Offset, .5f);
        for (; ; )
        {
            yield return null;
            if (InputToken.Direction.y < .5f || Mathf.Abs(InputToken.Direction.x) > .25f)
                goto neutral;
        }
    lookdown:
        cameraFollow.ShiftFocus(baseOffset - Offset, .5f);
        for (; ; )
        {
            yield return null;
            if (InputToken.Direction.y > -.5f || Mathf.Abs(InputToken.Direction.x) > .25f)
                goto neutral;
        }
    }
}
