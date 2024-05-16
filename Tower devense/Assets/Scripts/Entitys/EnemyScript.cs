using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour, EnemyInterface
{
    private int health;

    public EnemyData data;
    public HealthBarScript healthBar;

    private int index = 1;
    private Vector3 targetPosition;
    
    private Vector2Int[] path;
    private LevelScript levelScript;
    private LevelManagerGameScript levelManager;

    private float timer = 0;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        health = data.health;
        healthBar.setMaxHealth(health);

        targetPosition = levelScript.fieldToCoordinate(path[index]);
    }

    public int getHealth()
    {
        return health;
    }
    
    public int getProgress()
    {
        return index;
    }
    
    public void setData(Vector2Int[] p, LevelScript l, LevelManagerGameScript lm)
    {
        path = p;
        levelScript = l;
        levelManager = lm;
    }
    
    public EnemyData getData()
    {
        return data;
    }

    public void getDamage(float damage)
    {
        health -= (int)damage;
        if (health <= 0)
        {
            levelManager.EnemyDead(gameObject, this, false);
        }
        healthBar.setHealth(health);
        sprite.color = Color.red;
        timer = 1.5f;
    }

    private void destroyThis()
    {
        LevelManagerGameScript.activeEnemys.Remove(gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.Equals(targetPosition))
        {
            if (index == path.GetLength(0) - 1)
            {
                targetReached();
            }
            else
            {
                index++;
                targetPosition = levelScript.fieldToCoordinate(path[index]);
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, data.speed * Time.deltaTime);
        if (timer > 0)
        {
            timer -= 10 * Time.deltaTime;
        }
        else
        {
            sprite.color = Color.white;
        }
    }

    private void targetReached()
    {
        levelManager.EnemyDead(gameObject, this, true);
    }
}
