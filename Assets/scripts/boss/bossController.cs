using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class bossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private float attackWindUp = 0.8f;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackIndicator;

    private bool isAttacking;
    private float windUpTimer;
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

        UpdateAttackIndicator();
        FacePlayer();

        attackTimer -= Time.deltaTime;

        if (isAttacking) 
        { 
            windUpTimer -= Time.deltaTime;

            if (windUpTimer <= 0) 
            { 
                Attack();
            }

            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if(distance > attackRange)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }

        else
        {
            StartAttack();
        }
    }

    private void Attack()
    {
        Debug.Log("boss attack func called");
        playerhealth.TakeDamage(attackDamage);

        attackTimer = attackCoolDown;
        isAttacking = false;

        attackIndicator.SetActive(false);

        Debug.Log("boss attacked");
        
    }

    private void StartAttack()
    {
        if (attackTimer > 0f)
        {
            Debug.Log("attack cooldown: " + attackTimer);
            return;
        }
         

        isAttacking = true;
        windUpTimer = attackWindUp;
        animator.SetTrigger("Attack");
        attackIndicator.SetActive(true);

        Debug.Log("boss started attack");
    }

    private void UpdateAttackIndicator()
    {
        if (!isAttacking || attackIndicator == null || player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            attackIndicator.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void FacePlayer()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
}
