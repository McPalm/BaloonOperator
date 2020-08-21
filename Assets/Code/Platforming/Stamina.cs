using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{

    PlatformingCharacter pc;

    public bool HasStamina => exhausted == false && currentstamina > 0f;

    float currentstamina = 0;
    public float maxStamina = 1f;

    bool exhausted = false;

    public UnityEvent<float> OnValueChanged;


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlatformingCharacter>();
        pc.OnJump += Pc_OnJump;
        pc.OnWallJump += Pc_OnWallJump;
    }

    private void Pc_OnWallJump()
    {
        currentstamina -= .2f;
    }

    private void Pc_OnJump()
    {
        // currentstamina -= .125f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pc.Climbing)
        {
            if (pc.VMomentum > -.1f)
                currentstamina -= Time.fixedDeltaTime * (pc.VMomentum * .15f + .1f);
        }
        else if (pc.Grounded)
            currentstamina += Time.fixedDeltaTime * 2f;


        if (currentstamina <= 0f)
            exhausted = true;
        if (currentstamina >= maxStamina)
            exhausted = false;

        pc.allowWallJump = HasStamina;
        pc.allowClimb = HasStamina;

        currentstamina = Mathf.Clamp(currentstamina, 0f, maxStamina);

        OnValueChanged.Invoke(currentstamina / maxStamina);
    }
}
