using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float speed;
    public int health;
    public int damage;
    public int reward;
    public float SpawnRate;
}
