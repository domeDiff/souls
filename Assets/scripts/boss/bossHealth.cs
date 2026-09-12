using System;
using UnityEngine;

public class bossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1000;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Boss Health: " +  currentHealth);

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("BOSS DEFEATED");

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
