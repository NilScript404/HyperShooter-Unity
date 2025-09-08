using UnityEngine;

// another way of just injecting the movement object
// instead, we could not write this, and just manually drag and drop the playermovement in the components
// of this class
[RequireComponent(typeof(PlayerMovement))]
public class PlayerDetection : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header(" Setting ")]
    [SerializeField] private float detectionRadius;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (GameManager.instance.IsGameState())
            DetectStuff();
    }

    private void DetectStuff()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in detectedObjects)
        {
            if (collider.CompareTag("WarzoneEnter")) // so we check if we are colliding with the right thing
                EnterWarzoneCollider(collider);
            else if (collider.CompareTag("Finish"))
                HitFinishLine();

            if (collider.TryGetComponent(out CheckPoint checkpoint))
            {
                checkpoint.Interact();
            }
        }
    }

    private void EnterWarzoneCollider(Collider warzoneTriggerCollider)
    {
        Warzone warzone = warzoneTriggerCollider.GetComponentInParent<Warzone>();
        playerMovement.EnterWarzoneCallback(warzone);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void HitFinishLine()
    {
        Debug.Log("finish line");
        playerMovement.HitFinishLine();
    }
}
