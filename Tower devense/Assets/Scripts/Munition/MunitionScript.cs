using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunitionScript : MonoBehaviour, Munition
{
    private GameObject target;
    private float moveSpeed;
    private float damage;

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.GetComponent<EnemyInterface>().getDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Vector3 targ = target.transform.position;
            Vector3 objectPos = transform.position;
            transform.position = Vector3.MoveTowards(objectPos, targ, moveSpeed * Time.deltaTime);
            
            targ.z = 0f;
            
            targ.x -= objectPos.x;
            targ.y -= objectPos.y;
 
            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        }
    }

    public void setDat(float damage, float speed, GameObject targetPosition)
    {
        this.damage = damage;
        moveSpeed = speed;
        target = targetPosition;
    }

    private void Start()
    {
        SoundManagerScript.instance.play("Arrow");
    }
}
