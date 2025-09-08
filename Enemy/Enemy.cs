using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    enum State { Alive, Dead }
    private State state;

    [Header(" Elements ")]
    [SerializeField] private CharacterRagdoll characterRagdoll;
    [SerializeField] private CharacterIK characterIK;
    [SerializeField] private EnemyShooter enemyShooter;
    private PlayerMovement playerMovement;

    [Header(" Actions ")]
    public static Action onDead;

    void Start()
    {
        state = State.Alive;

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        characterIK.ConfigureIK(playerMovement.GetEnemyTarget());
    }

    void Update()
    {
        if (true == false) { }
    }

    public void TakeDamage()
    {
        if (state == State.Dead)
            return;

        Die();
    }

    public void Die()
    {
        state = State.Dead;
        onDead?.Invoke();

        characterRagdoll.Ragdollify();
    }

    public void ShootAtPlayer()
    {
        if (state == State.Dead)
            return;

        enemyShooter.TryShooting();
    }

    public bool IsDead()
    {
        return state == State.Dead;
    }
}
