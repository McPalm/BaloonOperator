using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImage : MonoBehaviour
{
    SpriteRenderer[] afterImages;
    SpriteRenderer mySprite;

    int qty = 8;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        afterImages = new SpriteRenderer[qty];
        for(int i = 0; i < qty; i++)
        {
            var o = new GameObject($"AfterImage{i}");
            var ren = o.AddComponent<SpriteRenderer>();
            ren.sprite = mySprite.sprite;
            afterImages[i] = ren;
            ren.sortingLayerID = mySprite.sortingLayerID;
            ren.sortingOrder = mySprite.sortingOrder - qty + i;
        }
    }

    Vector3 lastPosition;
    Vector3 lastSize;
    Quaternion lastRotation;

    private void FixedUpdate()
    {
        mySprite = GetComponent<SpriteRenderer>();

        var angleDelta = Quaternion.Angle(lastRotation, transform.rotation);
        // var positionDelta = Vector2.SqrMagnitude(transform.position - lastPosition);
        var show = angleDelta > 10f;


        for(int i = 0; i < qty; i++)
        {
            var ren = afterImages[i];
            if (show)
            {
                var f = (float)i / qty;
                ren.transform.position = Vector3.Lerp(lastPosition, transform.position, f);
                ren.transform.localScale = Vector3.Lerp(lastSize, transform.lossyScale, f);
                ren.transform.rotation = Quaternion.Lerp(lastRotation, transform.rotation, f);
                
                ren.sprite = mySprite.sprite;
                ren.enabled = mySprite.enabled;
                ren.color = Color.Lerp(new Color(1f, 1f, 1f, .5f), mySprite.color, f);
            }
            else
            {
                ren.enabled = false;
            }
        }


        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastSize = transform.lossyScale;
    }

    private void OnEnable()
    {
        if(mySprite != null)
        {
            foreach(var sprite in afterImages)
            {
                sprite.enabled = false;
            }
        }
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastSize = transform.lossyScale;
    }

    private void OnDisable()
    {
        foreach (var sprite in afterImages)
        {
            sprite.enabled = false;
        }
    }

}
