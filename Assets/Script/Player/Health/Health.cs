using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health: MonoBehaviour 
{
    [HideInInspector]
    public float baseHealth;

    public float CurrentHealth { get; private set;}

    private void Awake()
    {
        CurrentHealth = GetHealth();
    }

 
    public void DamageTaken(float _damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, GetHealth());
        DamagePopup.Create(transform.position,-_damage, false);
        if (CurrentHealth == 0)
        {
            Debug.Log("You die!");
        }
    }

    public void Healing(float hp)
    {
        if (CurrentHealth + hp > GetHealth())
        {
            hp = GetHealth() - CurrentHealth;
        }   
        CurrentHealth = Mathf.Clamp(CurrentHealth + hp, 0, GetHealth());
        DamagePopup.Create(transform.position, hp, false);  
    }
    public float GetHealth()
    {
        return baseHealth;
    }
    public void RestoreFullHealth()
    {
        CurrentHealth = GetHealth();
    }
}
