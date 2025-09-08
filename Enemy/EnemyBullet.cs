using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float detectionRadius;
    private Vector3 velocity;

    void Start()
    {

    }

    void Update()
    {
        Move();

        CheckForPlayer();
    }

    private void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void Configure(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void CheckForPlayer()
    {
        Collider[] detectedPlayer = Physics.OverlapSphere(transform.position, detectionRadius, playerMask);

        foreach (Collider playerCollider in detectedPlayer)
            playerCollider.GetComponent<PlayerMovement>().TakeDamage();

    }
}
