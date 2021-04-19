using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // float plane;
    float ease = 1f;
    float slowEase = 1f;
    float Ease => Mathf.Min(ease, slowEase);

    public Mobile[] Follow;
    public Transform CameraOffsetRoot;
    Vector3 Offset=> CameraOffsetRoot.localPosition;
    Vector3 DesiredOffset;

    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;

    private void Start()
    {
        // Snap();
        DesiredOffset = Offset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraOffsetRoot.localPosition = DesiredOffset;

        Vector3 average = Vector3.zero;
        for(int i = 0; i < Follow.Length; i++)
        {
            average += Follow[i].transform.position;
        }
        average *= 1f / Follow.Length;
           
        transform.position = new Vector3(average.x, average.y);//Vector3.Lerp(transform.position,  new Vector3(x, y), Ease);
    }

    public void ShiftFocus(Vector2 direction, float duration = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(ShiftFocusRoutine(direction, duration));
    }

    IEnumerator ShiftFocusRoutine(Vector3 direction, float duration = 1f)
    {
        direction.z = -10f;
        yield return null;
        Vector3 start = CameraOffsetRoot.localPosition;
        
        for(float f = 0; f < duration; f += Time.deltaTime)
        {
            yield return null;
            DesiredOffset = Vector3.Lerp(start, direction, f / duration);
        }
        DesiredOffset = direction;
    }
}
