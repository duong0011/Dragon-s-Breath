using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsControl : MonoBehaviour
{
    
    public Stats data;
    private Health playerHealth;

    [SerializeField]
    private AudioSource audioLevelUp;
    private void Awake()
    {
        data.OnRestoreFullHealth += HandlRestoreFullHealth;
        playerHealth = GetComponent<Health>();
        HealthCaculate();
        data.levelUpEvent.AddListener(TriggerLevelUpAnimation);
    }

    private void TriggerLevelUpAnimation(float arg0)
    {
        audioLevelUp.Play();
    }

    private void HandlRestoreFullHealth()
    {
        playerHealth.RestoreFullHealth();
    }

    private void HealthCaculate()
    {
       playerHealth.baseHealth = data.GetStat(Stat.MaxHealth);

    }
    public void AddExperience(float exp)
    {
        data.AddExperience(exp);
    }

}
