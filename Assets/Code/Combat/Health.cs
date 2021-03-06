﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Health : MonoBehaviour
{
    public int MaxHealth = 1;
    public int iFrames = 0;

    float IFrameTime = 0f;
    public bool HasIFrames => Time.timeSinceLevelLoad < IFrameTime;

    public int HealthLost { get; private set; }
    public int CurrentHealth => MaxHealth - HealthLost;

    public event System.Action<DamageData> OnHurt;
    public event System.Action<DamageData> OnHeal;
    public event System.Action OnZeroHealth;
    public event System.Action<int, DamageProperties> OnChangeTrueHealth;


    public event System.Action<DamageData> ApplyDefence; // invoked before damage is applied, defences may modify its effect

    public UnityEvent<float> OnChangeHealth;
    public UnityEvent<int> OnStruck;

    /// <summary>
    /// make it so that this isntance does not accept Hurt or Heal messages. Intended to manage authority over networking
    /// Can be overriden in health or hurt if appropiate.
    /// </summary>
    public bool blockTriggers = false;

    private void Start()
    {
        NotifyChangeHealthObservers();
    }

    public void Hurt(DamageData data)
    {
        ApplyDefence?.Invoke(data);
        if (data.damage < 1)
            return;
        if (HasIFrames)
            return;
        OnStruck.Invoke(data.damage);
        OnHurt?.Invoke(data);
        if (data.reject)
            return;
        if (iFrames > 0)
            IFrameTime = Time.timeSinceLevelLoad + Time.fixedDeltaTime * iFrames;
        HealthLost += data.damage;
        NotifyChangeHealthObservers();
    }

    public void Heal(DamageData data)
    {
        if (data.damage < 1)
            return;
        OnHeal?.Invoke(data);
        if (data.reject)
            return;
        HealthLost -= data.damage;
        HealthLost = Mathf.Max(HealthLost, 0);
        NotifyChangeHealthObservers();
    }

    public void SetHealth(int lost, DamageProperties props)
    {
        HealthLost = lost;
        NotifyChangeHealthObservers();
        if (HealthLost >= MaxHealth)
            OnZeroHealth?.Invoke();
        OnChangeTrueHealth?.Invoke(CurrentHealth, props);
    }

    void NotifyChangeHealthObservers() => OnChangeHealth.Invoke((float)CurrentHealth / (float)MaxHealth);
}
