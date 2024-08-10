using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigBang : Skill
{
    [SerializeField] Transform[] meteorite;
    public float radiusExplode;
    private float timeExist;
    private void Update()
    {
        timeExist += Time.deltaTime;
        if(timeExist >= 5f)
        {
            for (int i = 0; i < meteorite.Length; i++)
            {
                meteorite[i].GetComponent<Collider2D>().enabled = false;
                meteorite[i].gameObject.SetActive(false);
            }
            IsActive = false;
            
        }
    }
    public Vector2 GetRandomPositionAround(float radius)
    {
        float angle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(0f, radius);
        float x = caster.position.x + randomRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = caster.position.y + randomRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }

    public override IEnumerator Activate()
    {
        timeExist = 0;
        gameObject.SetActive(true);
        IsActive = true;
        for (int i = 0; i < meteorite.Length; i++)
        {
            meteorite[i].gameObject.SetActive(true);
            meteorite[i].position = GetRandomPositionAround(radiusExplode);
        }
        StartCoroutine(CooldownRoutine());

        yield break;
    }


}
