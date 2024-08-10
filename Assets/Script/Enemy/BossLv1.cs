using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossLv1 : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] private Transform castPoint;
    [SerializeField] private List<GameObject> listSkill;

    private FlyingEnemy flyingEnemy;

    private Skill curSkill;
    private void Awake()
    {
        curSkill = listSkill[0].GetComponent<Skill>();
        flyingEnemy = GetComponent<FlyingEnemy>();
    }

    private void Update()
    {
        if (flyingEnemy.Player != null && Vector2.Distance(flyingEnemy.Player.position, transform.position) <= flyingEnemy.distanceAttack + 0.2f)
        {
            
            for (int i = 0; i < listSkill.Count; i++)
            {
                if (listSkill[i].GetComponent<Skill>().IsActive == false && listSkill[i].GetComponent<Skill>().IsOnCooldown == false && curSkill.IsActive == false)
                {
                    Debug.Log(1);
                    StartCoroutine(listSkill[i].GetComponent<Skill>().Activate());
                    if (!listSkill[i].GetComponent<Skill>().CanCast)
                    {
                        continue;
                    }
                    curSkill = listSkill[i].GetComponent<Skill>();
                    break;
                }
            }
        }
    }
    public void ResetSkill()
    {
        for (int i = 0; i < listSkill.Count; i++)
        {
            listSkill[i].GetComponent<Skill>().ReseColdDown();
        }
    }
}