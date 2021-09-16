using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Character
{
    [SerializeField]
    private float maxHealth;

    private float currentHealth;

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        currentHealth = Mathf.Max(0, currentHealth);
    }

    public void TakeHealth(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
    }


}
