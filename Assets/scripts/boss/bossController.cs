using UnityEngine;
using UnityEngine.Rendering;

public class bossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackCoolDown = 2f;
    private Transform player;
    private playerHealth playerhealth;
    private float attackTimer;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.Log("Tag not found");
            return;
        }

        player = playerObject.transform;
        playerhealth = player.GetComponent<playerHealth>();
    }

    void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if(distance > attackRange)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }

        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (attackTimer > 0f)
            return;

        playerhealth.TakeDamage(attackDamage);

        attackTimer = attackCoolDown;

        Debug.Log("BOSS ATTACKED");
        
    }
}
