using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HesllishSlash : Skill
{
    [Header("Object")]
    [SerializeField] private Transform wawe;
    [SerializeField] private Transform endPoint;
    public float distanceFly;
    [SerializeField] private float speedWawe;
    [SerializeField] private Transform[] holyLights;
    

    private Vector2 castPointPos;
    private Animator animator;
    
    private bool isHolyLightCast = false;
    private bool isSkillEnd = false;
    private float lengthRequire;
    private Vector3 startPosition;
    private float playerY;
    private void Awake()
    {
        castPointPos = caster.GetChild(0).transform.position;
        animator = wawe.GetComponent<Animator>();
        

    }
    private void Update()
    {
        if (isSkillEnd)
        {
            IsActive = false;
            return;
        }
        // bay ve phia player
        //float direction = Mathf.Sign(Mathf.Sign(player.position.x - caster.transform.position.x));
        wawe.position = Vector3.MoveTowards(wawe.position, endPoint.position, speedWawe * Time.deltaTime);
        if (wawe.position == endPoint.position )
        {
            if(isHolyLightCast == false)
            {
                isHolyLightCast = true;
                wawe.gameObject.SetActive(false);
                endPoint.gameObject.SetActive(false);
                StartCoroutine(CallHolyLight());
            }
        }
    }
    private IEnumerator CallHolyLight()
    {
        for (int i = 0; i < holyLights.Length; ++i)
        {
            holyLights[i].gameObject.SetActive(true);
            
            holyLights[i].position = new Vector3(startPosition.x + (float)(i+1) * lengthRequire/holyLights.Length, playerY, 0);
            yield return new WaitForSeconds(0.3f);
            holyLights[i].gameObject.SetActive(false);
        }
        StartCoroutine(CooldownRoutine());
        isSkillEnd = true;
       
    }
    public override IEnumerator Activate()
    {
        CanCast = caster.GetComponent<Enemy>().Player.GetComponent<PlayerMovement>().IsGrounded();
        if (CanCast == false)
        {
            yield break;
          
        }
        transform.gameObject.SetActive(true);
        IsActive = true;
        wawe.position = castPointPos;
        isHolyLightCast = false;
        isSkillEnd = false;
        wawe.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);
        animator.SetTrigger("surfing");
        wawe.position = caster.position;
        startPosition = wawe.position;
        playerY = caster.GetComponent<Enemy>().Player.GetChild(1).position.y;
        //lay  huong dung cua player de tuy chinh end point
        lengthRequire = caster.transform.localScale.x < 0 ? distanceFly : -distanceFly;
        endPoint.position = new Vector3(wawe.position.x + lengthRequire, wawe.position.y, 0);
        yield return new WaitForSeconds(0);
    }
    


}
