using System;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
  
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("Player has died.");
        // Add death logic here (e.g., respawn, game over screen, etc.)
    }

    void Update()
    {
        
    }
}
