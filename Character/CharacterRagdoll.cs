using UnityEngine;

public class CharacterRagdoll : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody[] rigidBodies;

    [Header(" Settings ")]
    [SerializeField] private float ragdollForce;



    void Start()
    {
        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
    }

    void Update()
    {

    }

    public void Ragdollify()
    {
        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            // still dont understand why "up"
            rigidBody.AddForce((Vector3.up + Random.insideUnitSphere) * ragdollForce); 
        }

        animator.enabled = false;
        mainCollider.enabled = false;
    }
}
