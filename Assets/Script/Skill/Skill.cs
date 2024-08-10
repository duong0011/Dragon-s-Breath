using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected string skillName;
    [SerializeField] protected float cooldown;
    [SerializeField] protected int damageMultiplier;
    [SerializeField] protected Transform caster;

    protected bool hit = false;
    private bool isOnCooldown = false;
    private bool isActive = false;
    private float totalDamge;
    public bool CanCast {get; protected set; } = true;

    private void Start()
    {
        if (caster.GetComponent<Enemy>() != null)
        {
            totalDamge = caster.GetComponent<Enemy>().Damage * damageMultiplier;
        }
    }
    public bool IsOnCooldown
    {
        get { return isOnCooldown; }
    }
    public bool IsActive
    {
        set { isActive = value; }
        get { return isActive; }
    }
    public virtual IEnumerator Activate() {
        yield break;
    }

    protected IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
   
    public void HandleChildCollision(Collider2D col)
    {
        if (col.gameObject.layer == 9 && caster.gameObject.layer == 10)
        {
            col.GetComponent<Health>().DamageTaken(totalDamge);
        }
    }
    public void ReseColdDown()
    {
        isOnCooldown = false;
        isActive = false;
    }
}
