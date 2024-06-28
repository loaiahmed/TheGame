using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float maxHP;
    private float currentHP;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        healthBar.updateHealthBar(currentHP, maxHP);
    }

    public void recieveDmg(float dmg){
        currentHP -= dmg;
        healthBar.updateHealthBar(currentHP, maxHP);
        if(currentHP <= 0){
            Destroy(gameObject);
        }
    }
}
