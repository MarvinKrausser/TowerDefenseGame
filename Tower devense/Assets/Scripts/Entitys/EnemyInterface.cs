using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyInterface
{
    public void setData(Vector2Int[] p, LevelScript l, LevelManagerGameScript lm);
    public EnemyData getData();
    public void getDamage(float damage);

    public int getHealth();

    public int getProgress();
}
