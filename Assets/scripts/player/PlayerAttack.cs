using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 25;

    private bool hasHit;

    private void OnEnable()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!other.CompareTag("Boss"))
            return;

        bossHealth health = other.GetComponent<bossHealth>();

        if (health == null)
            return;

        health.TakeDamage(attackDamage);

        hasHit = true;

        Debug.Log("player hit boss");
        
    }
}
