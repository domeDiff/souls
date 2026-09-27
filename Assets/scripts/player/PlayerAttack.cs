using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Boss"))
            return;

        bossHealth health = other.GetComponent<bossHealth>();

        if(health != null)
        {
            health.TakeDamage(attackDamage);

            Debug.Log("player hit boss");
        }
    }
}
