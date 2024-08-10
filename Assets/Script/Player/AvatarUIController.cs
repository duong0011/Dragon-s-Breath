using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerStatsControl playerStatsControl;

    [SerializeField] GameObject level;
    private TMP_Text currentLevel;
    private void Awake()
    {
        currentLevel = level.GetComponent<TMP_Text>();
        //if (currentLevel == null) Debug.Log(1);
        ChangeLevel(playerStatsControl.data.GetStat(Stat.Level));
    }
    private void OnEnable()
    {
        playerStatsControl.data.levelUpEvent.AddListener(ChangeLevel);
    }
    private void OnDisable()
    {
        playerStatsControl.data.levelUpEvent.RemoveListener(ChangeLevel);
    }
    private void ChangeLevel(float level)
    {
        currentLevel.SetText(level.ToString());
    }
}
