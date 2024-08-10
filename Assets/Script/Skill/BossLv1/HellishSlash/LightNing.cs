using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightNing : MonoBehaviour
{
    [SerializeField] HesllishSlash mainSkill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mainSkill.HandleChildCollision(collision);
    }
}
