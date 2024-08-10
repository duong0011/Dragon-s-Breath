using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManeger : MonoBehaviour
{
    [SerializeField] private List<Transform> enemies;
    [SerializeField] private Collider2D playerCol;
    private float[] timeDeath;
    private float[] respawnTime;
    // Update is called once per frame
    private void Start()
    {
        timeDeath = new float[enemies.Count];
        respawnTime = new float[enemies.Count];
        SetUp();
    }
    private void SetUp()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            respawnTime[i] = enemies[i].GetComponent<Enemy>().localStats[Stat.RespawnTime];
            Physics2D.IgnoreCollision(enemies[i].GetComponent<Collider2D>(), playerCol);
            enemies[i].GetComponent<Enemy>().OnDropItem += HanldItemsDrop;
            
        }
    }

    private void HanldItemsDrop(EnemyDropItemSO listItem,  Collider2D col)
    {
        StartCoroutine(DropItem(listItem, col));
    }

    private IEnumerator DropItem(EnemyDropItemSO listItem, Collider2D col)
    {
       
        foreach (var item in listItem.Items)
        {
            if (item.item != null)
            {
                var itemComponent = item.item.GetComponent<Item>();
                if (itemComponent != null && itemComponent.InventoryItem.IsStackable)
                {
                    itemComponent.Quantity = UnityEngine.Random.Range(0, item.maxQuantityDrop);
                }
                Bounds bounds = col.bounds;
                Vector2 dropPosition = new(bounds.center.x, bounds.max.y);
                GameObject itemDropped = Instantiate(item.item.gameObject, dropPosition, Quaternion.identity);
                Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.5f, 1f)).normalized;
                itemDropped.GetComponent<Rigidbody2D>().AddForce(randomDirection * 400);
                itemDropped.TryGetComponent<Collider2D>(out Collider2D itemCollider);
                if (itemCollider != null)
                {
                    Physics2D.IgnoreCollision(itemCollider, playerCol);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!IsObjectActive(enemies[i].gameObject))
            {
                if (timeDeath[i] >= respawnTime[i])
                {
                    Enemy enemy = enemies[i].GetComponent<Enemy>();
                    enemy.gameObject.SetActive(true);
                    Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), playerCol);
                    enemy.Respanw();
                    timeDeath[i] = 0f;
                }
                else
                {
                    timeDeath[i] += Time.deltaTime;
                }
            }
        }
    }
    private bool IsObjectActive(GameObject obj)
    {
        return obj.activeInHierarchy;
    }
}
