using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerEnemyTrigger : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private LineRenderer shootingLine;
    private PlayerMovement playerMovement;


    [Header(" Settings ")]
    [SerializeField] private LayerMask enemiesMask;
    private List<Enemy> currentEnemies = new List<Enemy>();
    private bool canCheckForShootingEnemies;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        PlayerMovement.OnEnterWarzone += EnteredWarzoneCallback;
        PlayerMovement.OnExitWarzone += ExitedWarzoneCallback;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnEnterWarzone -= EnteredWarzoneCallback;
        PlayerMovement.OnExitWarzone -= ExitedWarzoneCallback;
    }

    void Start()
    {

    }

    void Update()
    {
        if (canCheckForShootingEnemies)
            CheckForShootingEnemies();
    }

    private void EnteredWarzoneCallback()
    {
        canCheckForShootingEnemies = true;
    }

    private void ExitedWarzoneCallback()
    {
        canCheckForShootingEnemies = false;
    }

    private void CheckForShootingEnemies()
    {
        // need the world space vector of the origin of line
        Vector3 rayOrigin = shootingLine.transform.TransformPoint(shootingLine.GetPosition(0));
        Vector3 rayLastPoint = shootingLine.transform.TransformPoint(shootingLine.GetPosition(1));

        Vector3 rayDirection = (rayLastPoint - rayOrigin).normalized;

        float maxDistance = Vector3.Distance(rayOrigin, rayLastPoint);

        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection, maxDistance, enemiesMask);

        for (int i = 0; i < hits.Length; i++)
        {
            Enemy currentEnemy = hits[i].collider.GetComponent<Enemy>();
            if (!currentEnemies.Contains(currentEnemy))
                currentEnemies.Add(currentEnemy);
        }

        List<Enemy> enemiesToRemove = new List<Enemy>();

        foreach (Enemy enemy in currentEnemies)
        {
            bool enemyFound = false;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<Enemy>() == enemy)
                {
                    enemyFound = true;
                    break;
                }
            }

            if (!enemyFound)
            {
                if (enemy.transform.parent == playerMovement.GetCurrentWarzone().transform)
                    enemy.ShootAtPlayer();
                // currentEnemies.Remove(enemy);
                enemiesToRemove.Add(enemy);
            }
        }

        // if we try to directly remove the enemy in the previous loop when the "!enemyFound" is true
        // from the currentEnemeis List, it would cuase an issue, since we are already iterating over
        // currentEnemies using a foreach loop, so we cant directly remove the enemy in that foreach loop
        // thats why we create a temporary list of enemies before the previous foreach, and store the enemies
        // we find in that list, and after the loop is over, we remove any found enemy from the currentEnemies
        foreach (Enemy enemy in enemiesToRemove)
        {
            currentEnemies.Remove(enemy);
        }
    }
}
