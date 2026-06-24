using UnityEngine;
public class BossController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    private Transform player;
    private bool movingRight = true;
    private bool chasingPlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            if (transform.position.x >= rightEdge.position.x) movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x <= leftEdge.position.x) movingRight = true;
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