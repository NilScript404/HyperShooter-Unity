using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MissionManager))]
public class MissionTracker : MonoBehaviour
{
    private MissionManager missionManager;

    public void Awake()
    {
        missionManager = GetComponent<MissionManager>();
        Enemy.onDead += OnEnemyDied;
        OutfitShopManager.onOutfitUnlocked += OnOutfitPurchase;
    }

    private void OnDestroy()
    {
        Enemy.onDead -= OnEnemyDied;
        OutfitShopManager.onOutfitUnlocked -= OnOutfitPurchase;
    }

    private void OnEnemyDied()
    {
        List<int> unCompletedMissionsIndex = missionManager.GetUncompletedMissionsIndex();
        Mission[] missions = missionManager.GetMissions();

        foreach (int index in unCompletedMissionsIndex)
        {
            Mission mission = missions[index];
            if (mission.missionType == MissionType.Kill)
            {
                missionManager.UpdateMissionProgress(index);
                break;
            }
        }
    }

    private void OnOutfitPurchase()
    {
        List<int> unCompletedMissionsIndex = missionManager.GetUncompletedMissionsIndex();
        Mission[] missions = missionManager.GetMissions();

        foreach (int index in unCompletedMissionsIndex)
        {
            Mission mission = missions[index];
            if (mission.missionType == MissionType.Outfit)
            {
                missionManager.UpdateMissionProgress(index);
                break;
            }
        }
    }
}
