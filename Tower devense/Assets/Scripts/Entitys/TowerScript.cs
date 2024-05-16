using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public TowerData data;
    public GameObject munition;

    private float timer = 0;

    private GameObject giveEnemyDistance()
    {
        GameObject nearestEnemy = null;
        float distanceCopy = float.MaxValue;
        foreach (GameObject enemy in LevelManagerGameScript.activeEnemys)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < distanceCopy && distance <= data.range)
            {
                nearestEnemy = enemy;
                distanceCopy = distance;
            }
        }

        return nearestEnemy;
    }

    private GameObject giveEnemyHealthiest()
    {
        GameObject healthiestEnemy = null;
        float healthCopy = 0;
        foreach (GameObject enemy in LevelManagerGameScript.activeEnemys)
        {
            int health = enemy.GetComponent<EnemyInterface>().getHealth();
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (health > healthCopy && distance <= data.range)
            {
                healthiestEnemy = enemy;
                healthCopy = health;
            }
        }

        return healthiestEnemy;
    }
    
    private GameObject giveEnemyWeakest()
    {
        GameObject weakestEnemy = null;
        float healthCopy = float.MaxValue;
        foreach (GameObject enemy in LevelManagerGameScript.activeEnemys)
        {
            int health = enemy.GetComponent<EnemyInterface>().getHealth();
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (health < healthCopy && distance <= data.range)
            {
                weakestEnemy = enemy;
                healthCopy = health;
            }
        }

        return weakestEnemy;
    }
    
    private GameObject giveEnemyMostProgress()
    {
        GameObject furthestEnemy = null;
        float progressCopy = 0;
        foreach (GameObject enemy in LevelManagerGameScript.activeEnemys)
        {
            int progress = enemy.GetComponent<EnemyInterface>().getProgress();
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (progress > progressCopy && distance <= data.range)
            {
                furthestEnemy = enemy;
                progressCopy = progress;
            }
        }

        return furthestEnemy;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= data.fireRate * Time.deltaTime;
        }
        else
        {
            GameObject enemyTarget = null;
            switch (LevelManagerGameScript.towerMode)
            {
                case 0:
                    enemyTarget = giveEnemyDistance();
                    break;
                case 1:
                    enemyTarget = giveEnemyHealthiest();
                    break;
                case 2:
                    enemyTarget = giveEnemyWeakest();
                    break;
                case 3:
                    enemyTarget = giveEnemyMostProgress();
                    break;
            }

            if (enemyTarget is not null)
            {
                attack(enemyTarget);
            }
        }
    }

    private void attack(GameObject enemy)
    {
        timer = 10;
        GameObject m = Instantiate(munition, transform.position, transform.rotation);
        m.GetComponent<Munition>().setDat(data.damage, data.speed, enemy);
    }
}
