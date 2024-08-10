using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class NuclearBall : MonoBehaviour
{
    private FiveBall par;
    private void Awake()
    {
        par = GetComponentInParent<FiveBall>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        par.HandleChildCollision(collision);
    }
}
