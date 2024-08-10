
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image currentHeal;

    private void Awake()
    {
        currentHeal.fillAmount = (float)(playerHealth.CurrentHealth) / (float)playerHealth.GetHealth();
    }

    private void Update()
    {
        currentHeal.fillAmount = (float)(playerHealth.CurrentHealth) / (float)playerHealth.GetHealth();
    }
}
