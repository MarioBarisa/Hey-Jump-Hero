using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    private Transform player;
    private bool movingRight = true;
    private bool chasingPlayer = false;
    private float leftBound;
    private float rightBound;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        BoxCollider2D area = GetComponentInParent<BoxCollider2D>();
        leftBound = area.bounds.min.x;
        rightBound = area.bounds.max.x;
    }

    private void Update()
    {
        if (chasingPlayer)
            ChasePlayer();
        else
            Patrol();
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.position.x >= rightBound) movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x <= leftBound) movingRight = true;
        }
    }

    private void ChasePlayer()
    {
        if (player.position.x > transform.position.x)
        {
            transform.position += Vector3.right * chaseSpeed * Time.deltaTime;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.position += Vector3.left * chaseSpeed * Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void SetChasing(bool value)
    {
        chasingPlayer = value;
    }
}