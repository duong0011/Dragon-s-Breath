using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    
    [SerializeField] private Skill mainSkill;
    private Animator animator;
    private void TriggerExplode()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Explode");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        mainSkill.HandleChildCollision(collision);
    }
}
