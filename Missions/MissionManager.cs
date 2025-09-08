using System.Collections.Generic;
using UnityEngine;

public enum MissionType { Kill, Headshot, Outfit,}

[System.Serializable]
public struct Mission
{
    public MissionType missionType;
    public int target;
    public int reward;
    public float progress;
}

public class MissionManager : MonoBehaviour
{

    public List<int> unCompletedMissionsIndex = new List<int>();
    public List<int> CompletedMissionsIndex = new List<int>();
    [Header(" Data ")]
    [SerializeField] private Mission[] missions;

    [Header(" Elements ")]
    [SerializeField] private MissionContainer missionContainerPrefab;
    [SerializeField] private Transform missionContainersParent;

    private void Awake()
    {
        MissionContainer.onRewardClaimed += MissionRewardClaimedCallback;
    }

    private void OnDestroy()
    {
        MissionContainer.onRewardClaimed -= MissionRewardClaimedCallback;
    }

    private void MissionRewardClaimedCallback(int missionIndex)
    {
        // save the state of the mission after claiming the reward 
        SetMissionCompleted(missionIndex);
        unCompletedMissionsIndex.Remove(missionIndex);

        int reward = missions[missionIndex].reward;
        CashManager.instance.AddCoins(reward);

        UpdateMissionState();
    }

    private void UpdateMissionState()
    {
        while (missionContainersParent.childCount > 0)
        {
            Transform t = missionContainersParent.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        CreateMissionContainers();
    }

    void Start()
    {
        CreateMissionContainers();
    }

    void Update()
    {

    }

    // private void CreateMissionContainers()
    // {
    //     for (int i = 0, unCompletedMissionsCount = 0, render = 0; i < missions.Length; i++)
    //     {
    //         // if (unCompletedMissionsCount >= 3)
    //         //     break;

    //         if (!IsMissionComplete(i) && !CompletedMissionsIndex.Contains(i))
    //         // if (!IsMissionComplete(i) && )
    //         {
    //             unCompletedMissionsIndex.Add(i);
    //             unCompletedMissionsCount++;
    //             if (render < 3)
    //             {
    //                 CreateMissionContainer(i);
    //                 render++;
    //             }
    //         }
    //     }
    // }

    private void CreateMissionContainers()
    {
        unCompletedMissionsIndex.Clear(); // Reset the list to rebuild accurately
        for (int i = 0, render = 0; i < missions.Length; i++)
        {
            if (!IsMissionComplete(i))
            {
                unCompletedMissionsIndex.Add(i);
                if (render < 3)
                {
                    CreateMissionContainer(i);
                    render++;
                }
            }
        }
    }


    private void CreateMissionContainer(int missionIndex)
    {
        MissionContainer missionContainer = Instantiate(missionContainerPrefab, missionContainersParent);
        Debug.Log("created missionContainer");
        string title = GetMissionTitle(missions[missionIndex]);
        string rewardString = missions[missionIndex].reward.ToString();
        float progress = GetMissionProgress(missionIndex);
        missions[missionIndex].progress = progress * missions[missionIndex].target;

        missionContainer.Configure(title, rewardString, progress, missionIndex);
    }

    private string GetMissionTitle(Mission mission)
    {
        switch (mission.missionType)
        {
            case MissionType.Kill:
                return "Kill " + mission.target.ToString() + " enemies";
            case MissionType.Headshot:
                return "Do " + mission.target.ToString() + " headshot";
            case MissionType.Outfit:
                return "Unlock " + mission.target.ToString() + " outfit";

            default:
                return "Blank";
        }
    }

    public List<int> GetUncompletedMissionsIndex()
    {
        return unCompletedMissionsIndex;
    }

    public Mission[] GetMissions()
    {
        return missions;
    }

    public void UpdateMissionProgress(int missionIndex)
    {
        Debug.Log("Updating the mission Progress index:  " + missionIndex);
        Mission mission = missions[missionIndex];
        float currentProgress = mission.progress;
        currentProgress++;

        float newProgress = (float)(currentProgress/ mission.target);
        SaveMissionProgress(missionIndex, newProgress); // a bit iffy

        missions[missionIndex].progress = currentProgress; // ??

        for (int i = 0; i < missionContainersParent.childCount; i++)
        {
            MissionContainer missionContainer = missionContainersParent.GetChild(i).GetComponent<MissionContainer>();

            if (missionContainer.GetKey() != missionIndex)
                continue;

            Debug.Log("updating the missionContainer");
            missionContainer.UpdateProgress(newProgress);
        }
    }

    private void SetMissionCompleted(int missionIndex)
    {
        PlayerPrefs.SetInt("Mission" + missionIndex, 1);
        CompletedMissionsIndex.Add(missionIndex);
    }

    private bool IsMissionComplete(int missionIndex)
    {
        // return PlayerPrefs.GetFloat("Mission" + missionIndex) >= 1;
        return PlayerPrefs.GetInt("Mission" + missionIndex) == 1;
    }

    private float GetMissionProgress(int missionIndex)
    {
        return PlayerPrefs.GetFloat("MissionProgress" + missionIndex);
    }

    private void SaveMissionProgress(int missionIndex, float newProgress)
    {
        PlayerPrefs.SetFloat("MissionProgress" + missionIndex, newProgress);
        PlayerPrefs.Save();
    }
}




