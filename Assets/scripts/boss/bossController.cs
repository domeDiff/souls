using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class bossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private GameObject attackHitBox;

    private bool isAttacking;
    private Transform player;
    private playerHealth playerhealth;
    private float attackTimer;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.Log("Tag not found");
            return;
        }

        player = playerObject.transform;
        playerhealth = player.GetComponent<playerHealth>();

        if (attackHitBox != null)
        {
            attackHitBox.SetActive(false);
        }

        // Make sure the attack indicator starts disabled.
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        UpdateAttackIndicator();
        FacePlayer();

        if (isAttacking)
        {
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > attackRange)
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

        attackTimer = attackCoolDown;
        isAttacking = false;

        attackIndicator.SetActive(false);

        Debug.Log("boss attacked");

    }

    private void StartAttack()
    {
        if (attackTimer > 0f)
            return;

        isAttacking = true;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if(direction.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (attackIndicator != null)
        {
            attackIndicator.SetActive(true);
        }

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

    public void AnimationAttackHit()
    {
        Debug.Log("AnimationAttackHit called");

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if(direction.sqrMagnitude > 0.01f)
        {
            attackHitBox.transform.rotation = Quaternion.LookRotation(direction);
        }

        if(attackHitBox != null)
        {
            attackHitBox.SetActive(true);
        }

        // Start cooldown.
        attackTimer = attackCoolDown;

        Debug.Log("Boss attack hitbox activated");
    }

    public void AnimationAttackEnd()
    {
        Debug.Log("AnimationAttackEnd called");

        if (attackHitBox != null)
        {
            attackHitBox.SetActive(false);
        }

        // Attack is finished.
        isAttacking = false;

        // Hide warning indicator.
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(false);
        }

        Debug.Log("attack ended");
    }
}