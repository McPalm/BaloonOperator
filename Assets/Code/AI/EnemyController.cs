using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Mobile Mobile { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Mobile = GetComponent<Mobile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mobile.TouchingWallDirection == Mobile.Forward)
        {
            Mobile.FaceRight = !Mobile.FaceRight;
        }
        Mobile.HMomentum = Mobile.Forward * 3;
    }
}
