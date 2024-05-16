using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;
    public LevelLoader levelLoader;

    public HealthBarScript healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    public void getDamage(int damage)
    {
        if (damage > 0 && (health - damage) > 0)
        {
            SoundManagerScript.instance.play("Quack");
        }
        health -= damage;
        healthBar.setHealth(health);
        if (health <= 0)
        {
            SoundManagerScript.instance.play("Death");
            SoundManagerScript.instance.stop("GameMusic");
            levelLoader.startTransition(3);
        }
    }
}
