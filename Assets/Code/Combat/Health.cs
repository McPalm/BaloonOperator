using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Health : MonoBehaviour
{
    public int MaxHealth = 1;

    public int HealthLost { get; set; }
    public int CurrentHealth => MaxHealth - HealthLost;

    public event System.Action<int, int> OnChange;
    public event System.Action<int> OnHurt;
    public event System.Action<int> OnHeal;
    public event System.Action OnZeroHealth;

    public UnityEvent<float> OnChangeHealth;

    private void Start()
    {
        OnChangeHealth.Invoke((float)CurrentHealth / (float)MaxHealth);
    }

    public void Hurt(int damage)
    {
        if (damage < 1)
            return;
        HealthLost -= damage;
        OnHurt(damage);
        OnChange?.Invoke(CurrentHealth, MaxHealth);
        if (HealthLost >= MaxHealth)
            OnZeroHealth?.Invoke();
        OnChangeHealth.Invoke(CurrentHealth);
    }

    public void Heal(int damage)
    {
        OnHeal(damage);
        if (damage > HealthLost)
            damage = HealthLost;
        if (damage < 1)
            return;
        OnChange?.Invoke(CurrentHealth, MaxHealth);
        OnChangeHealth.Invoke(CurrentHealth);
    }

    public void SetHealth(int lost, int change)
    {
        HealthLost = lost;
        OnChangeHealth.Invoke((float)CurrentHealth / (float) MaxHealth);
    }
}
