using UnityEngine;

public class bossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if(direction.sqrMagnitude > 0.1f)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
    }
}
