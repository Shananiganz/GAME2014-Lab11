using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float horizontalSpeed;
    public Transform inFrontPoint;
    public Transform aheadPoint;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;
    public bool isGroundAhead;
    public bool isObstacleAhead;
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        isObstacleAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, groundLayerMask);
        isGroundAhead = Physics2D.Linecast(groundPoint.position, aheadPoint.position, groundLayerMask);
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);

        if (isGrounded && isGroundAhead)
        {
            Move();
        }

        if (!isGroundAhead || isObstacleAhead)
        {
            Flip();
        }
    }

    public void Move()
    {
        transform.position += new Vector3(direction.x * horizontalSpeed * Time.deltaTime, 0.0f);
    }

    public void Flip()
    {
        var x = transform.localScale.x * -1.0f;
        direction *= -1.0f;
        transform.localScale = new Vector3(x, 1.0f, 1.0f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
        Gizmos.DrawLine(groundPoint.position, inFrontPoint.position);
        Gizmos.DrawLine(groundPoint.position, aheadPoint.position);
    }
}
