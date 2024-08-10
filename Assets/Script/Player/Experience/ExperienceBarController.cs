using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarController : MonoBehaviour
{
    [SerializeField]
    private PlayerStatsControl playerStatsControl;
    [SerializeField]
    private Image currentHealthBar;
    private void Awake()
    {
        float curPercentExp = playerStatsControl.data.GetStat(Stat.CurExp) / playerStatsControl.data.GetStat(Stat.BaseExperience) * (float)Math.Pow(playerStatsControl.data.GetStat(Stat.Level), 1.55f);
        ChangeFillAmountValue(curPercentExp);
    }
    private void OnEnable()
    {
        playerStatsControl.data.expChangeEvent.AddListener(ChangeFillAmountValue);
    }
    private void OnDisable()
    {
        playerStatsControl.data.expChangeEvent.RemoveListener(ChangeFillAmountValue);
    }
    private void ChangeFillAmountValue(float amount)
    {
        currentHealthBar.fillAmount = amount;
        currentHealthBar.color = GetColorBasedOnFillAmount(amount);
    }
    private Color GetColorBasedOnFillAmount(float fillAmount)
    {
        if (fillAmount < 0.15f)
        {
            return Color.red; // Red
        }
        else if (fillAmount < 0.3f)
        {
            return new Color(1.0f, 0.64f, 0.0f); // Orange
        }
        else if (fillAmount < 0.45f)
        {
            return Color.yellow; // Yellow
        }
        else if (fillAmount < 0.6f)
        {
            return Color.green; // Green
        }
        else if (fillAmount < 0.75f)
        {
            return Color.blue; // Blue
        }
        else if (fillAmount < 0.9f)
        {
            return new Color(0.29f, 0.0f, 0.51f); // Indigo (Chàm)
        }
        else
        {
            return new Color(0.56f, 0.0f, 1.0f); // Violet (Tím)
        }
    }
}
