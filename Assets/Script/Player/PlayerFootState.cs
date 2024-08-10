using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootState : MonoBehaviour
{
    
    [SerializeField] private Transform player;
    [Header("Befor Ground")]
    [SerializeField] private Vector2 beforeGroundCheckSize;
    [SerializeField] private LayerMask groundLayer;

    
    private Animator m_Animator;
    private BoxCollider2D playerBoxCollider;
 
   
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        playerBoxCollider = player.GetComponent<BoxCollider2D>();


    }

    private void Update()
    {
       
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        Vector3 colliderBottomCenter = playerBoxCollider.bounds.center;
        colliderBottomCenter.y -= playerBoxCollider.bounds.extents.y;
        transform.position = colliderBottomCenter;
        
    }
    public void EnableAni()
    {
        m_Animator.SetTrigger("secondJump");
    }
    
    public bool IsAboutToTouchGround()
    {
        return Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - (beforeGroundCheckSize.y/2.0f)), beforeGroundCheckSize, 0, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        //for before ground check
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - (beforeGroundCheckSize.y / 2.0f)), beforeGroundCheckSize);
       
    }

}
