using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckPoint : MonoBehaviour
{
    [SerializeField] private Transform player;
    private BoxCollider2D playerBoxCollider;
    // Start is called before the first frame update
    private void Awake()
    {
        playerBoxCollider = player.GetComponent<BoxCollider2D>();
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        Vector3 colliderBottomCenter = playerBoxCollider.bounds.center;
        colliderBottomCenter.x += playerBoxCollider.bounds.extents.x;
        
        colliderBottomCenter.x *= player.transform.localScale.x / Math.Abs(player.transform.localScale.x);
        transform.position = colliderBottomCenter;
    }
}
