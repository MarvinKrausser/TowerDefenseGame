using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveMunitionScript : MonoBehaviour, Munition
{
    private GameObject target;
    private float moveSpeed;
    private float damage;
    public GameObject explosion;

    private float explosionRadius = 1.4f;

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            explode();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void setDat(float damage, float speed, GameObject targetPosition)
    {
        this.damage = damage;
        moveSpeed = speed;
        target = targetPosition;
    }

    private void explode()
    {
        SoundManagerScript.instance.play("Explosion");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemyTag");
        for (int i = 0; i < enemys.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
            if (distance < explosionRadius)
            {
                enemys[i].GetComponent<EnemyInterface>().getDamage(damage / (distance + 1));
            }
        }
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
