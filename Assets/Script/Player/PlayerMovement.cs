using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxColider;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 groundCheckSize;
    private float horizotalGetAxis;
    private float curFace;
    private float tmpSpeed;
    public bool canMove = true;

    [Header("Jump")]
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform foot;
    [SerializeField] private int fastFallGravityScale;
    public int jumpCount;
    private bool triggerJump;
    private bool isFalling;

    [Header("WallSliding")]
    [SerializeField] private float wallSlideingSpeed;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask wallCheckLayer;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isWallSilding;
    private float timeOutWall;
    private bool isInTimeOutWall;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxColider = GetComponent<BoxCollider2D>();
        tmpSpeed = speed;
        curFace = transform.localScale.x;
        isFalling = false;
        UpdateBoxCollider();
    }
    private void Update()
    {
        
        horizotalGetAxis = canMove != false  ? Input.GetAxis("Horizontal") : 0;
        triggerJump = false;
        timeOutWall += Time.deltaTime;
        
        isInTimeOutWall = timeOutWall <= 0.1f;

        if (IsGrounded())
        {
            isFalling = false;
            jumpCount = 0;
            
        }
       
        WallSliding();
        
        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            Jump();
        }
        
        speed = IsGrounded() ? tmpSpeed : tmpSpeed/3.0f;
        UpdateAnimation();
        UpdateBoxCollider();
       
    }
    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizotalGetAxis * speed, body.velocity.y);
        body.gravityScale = (Input.GetKey(KeyCode.S)) ? fastFallGravityScale : body.velocity.y < 0 ? 8 : 1;
        var curAni = animator.GetCurrentAnimatorStateInfo(0);
    }

    private void LateUpdate()
    {
       Flip();
    }
    private void UpdateAnimation()
    {
        
        animator.SetBool("run", horizotalGetAxis != 0 && IsGrounded());
        animator.SetBool("isGrounded", IsGrounded());
        if(triggerJump)
        {
            animator.SetTrigger("jump");
        }
        if (body.velocity.y < 0.0f && isFalling == false && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !IsGrounded())
        {
            animator.SetTrigger("jump");
            isFalling = true;
        }    
    }

    private void UpdateBoxCollider()
    {
        Vector2 _size = GetComponent<SpriteRenderer>().sprite.bounds.size;
        boxColider.size = new Vector2(Math.Min(_size.x, 0.46f), Math.Max(0.83f, _size.y));
        boxColider.offset = GetComponent<SpriteRenderer>().sprite.bounds.center;
        

    }
    
    private void Flip()//flip sprite
    {
       
        if (horizotalGetAxis > 0.1f && canMove)
        {
            transform.localScale = new Vector3(curFace, transform.localScale.y, transform.localScale.z);
        }
        else if (horizotalGetAxis < -0.1f && canMove)
        {
            transform.localScale = new Vector3(curFace * -1.0f, transform.localScale.y, transform.localScale.z);
        }
       
    }
    private void Jump()
    {
        if (IsGrounded() || isInTimeOutWall)
        {
            //body.velocity = new Vector2(body.velocity.x, jumpPower);
            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            triggerJump = true;
        }
        else if(!IsGrounded() && jumpCount == 0)
        {

            //body.velocity = new Vector2(body.velocity.x, jumpPower);
            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            triggerJump = true;
            if (jumpCount >= 0)
            {
                
                foot.GetComponent<PlayerFootState>().EnableAni();
            }
            jumpCount++;
        }
    }
    private void WallSliding()
    {

        if (IsTouchingWall() && !IsGrounded() && body.velocity.y <= 0) {
            isWallSilding = true;
            timeOutWall = 0;
            jumpCount = 0;
        }
        else
        {
            isWallSilding = false;
        }
        if (isWallSilding) 
        { 
            body.velocity = new Vector2(body.velocity.x, -wallSlideingSpeed);
        }
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(foot.position, groundCheckSize, 0, groundLayer);
    }

    public bool IsTouchingWall()
    {
        return Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallCheckLayer);
    }
    public bool CanAttack()//can atack if not on wall 
    {
        return !isWallSilding && !IsTouchingWall();
    }

    private void OnDrawGizmosSelected()
    {
        //for wall check
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
        //for ground check
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawCube(foot.position, groundCheckSize);
    }
   
    
}
