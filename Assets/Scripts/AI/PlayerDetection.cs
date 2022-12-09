using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header("Sensing Suite")]
    public LayerMask collisionLayerMask;
    public Collider2D colliderName;
    public bool playerDetected;
    public bool LOS;
    public Transform playerTransform;
    public float playerDirection;
    public float enemyDirection;

    private Vector2 playerDirectionVector;

    // Start is called before the first frame update
    void Start()
    {
        playerDirectionVector = Vector2.zero;
        playerDirection = 0;
       
        playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
        playerDetected = false;
        LOS = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            var hits = Physics2D.Linecast(transform.position, playerTransform.position, collisionLayerMask);

            colliderName = hits.collider;

            playerDirectionVector = (playerTransform.position - transform.position);
            playerDirection = (playerDirectionVector.x > 0) ? 1.0f : -1.0f;
            enemyDirection = GetComponentInParent<EnemyController>().direction.x;

            LOS = (hits.collider.gameObject.name == "Player") && (playerDirection == enemyDirection);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            playerDetected = true;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = (LOS) ? Color.green : Color.red;

        if (playerDetected)
        {
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }

        Gizmos.color = (playerDetected) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 15.0f);
    }
}
