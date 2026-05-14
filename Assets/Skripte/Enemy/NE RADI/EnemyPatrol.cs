using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement Parameters")]
    [SerializeField] private float speed;

    private Vector3 initScale;
    private bool movingLeft;

    private void Awake()
    {
        initScale= enemy.localScale;
    }

    private void Update()
    {
        Debug.Log($"pos: {enemy.position.x}, left: {leftEdge.position.x}, right: {rightEdge.position.x}, movingLeft: {movingLeft}");
    
        if(movingLeft){
            if(enemy.position.x >= leftEdge.position.x){
                MovementInDirection(-1);
            }
            else
            {
                Flip();
            }
        }
        else
        {
            if(enemy.position.x <= rightEdge.position.x){
                MovementInDirection(1);
            }
            else
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        movingLeft= !movingLeft;
        transform.Rotate(0f, 180f, 0f);
    }

    private void MovementInDirection(int direction)
    {
        enemy.localScale= new Vector3(
            Mathf.Abs(initScale.x) * (movingLeft ? -1 : 1), 
            initScale.y, 
            initScale.z
        );

        enemy.position= new Vector3(
            enemy.position.x + Time.deltaTime * direction * speed,
            enemy.position.y,
            enemy.position.z
        );

    }

}
