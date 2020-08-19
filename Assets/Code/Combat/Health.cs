using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Health : MonoBehaviour
{
    public int MaxHealth = 1;

    public int HealthLost { get; private set; }
    public int CurrentHealth => MaxHealth - HealthLost;

    public event System.Action<DamageData> OnHurt;
    public event System.Action<DamageData> OnHeal;
    public event System.Action OnZeroHealth;

    public UnityEvent<float> OnChangeHealth;

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
        if (data.damage < 1)
            return;
        HealthLost += data.damage;
        OnHurt?.Invoke(data);
        if (HealthLost >= MaxHealth)
            OnZeroHealth?.Invoke();
        NotifyChangeHealthObservers();
    }

    public void Heal(DamageData data)
    {
        if (data.damage < 1)
            return;
        HealthLost -= data.damage;
        HealthLost = Mathf.Max(HealthLost, 0);
        OnHeal?.Invoke(data);
        NotifyChangeHealthObservers();
    }

    public void SetHealth(int lost, int change)
    {
        HealthLost = lost;
        NotifyChangeHealthObservers();
    }

    void NotifyChangeHealthObservers() => OnChangeHealth.Invoke((float)CurrentHealth / (float)MaxHealth);
}
