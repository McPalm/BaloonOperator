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

    public event System.Action<int, int> OnChange;
    public event System.Action<int> OnHurt;
    public event System.Action<int> OnHeal;
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

    public void Hurt(int damage)
    {
        if (damage < 1)
            return;
        Debug.Log("Was hurt!", gameObject);
        HealthLost += damage;
        OnHurt?.Invoke(damage);
        OnChange?.Invoke(CurrentHealth, MaxHealth);
        if (HealthLost >= MaxHealth)
            OnZeroHealth?.Invoke();
        NotifyChangeHealthObservers();
    }

    public void Heal(int damage, bool overrideAuthority = false)
    {
        OnHeal?.Invoke(damage);
        if (damage > HealthLost)
            damage = HealthLost;
        if (damage < 1)
            return;
        HealthLost -= damage;
        OnChange?.Invoke(CurrentHealth, MaxHealth);
        NotifyChangeHealthObservers();
    }

    public void SetHealth(int lost, int change)
    {
        HealthLost = lost;
        NotifyChangeHealthObservers();
    }

    void NotifyChangeHealthObservers() => OnChangeHealth.Invoke((float)CurrentHealth / (float)MaxHealth);
}
