using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MissionContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] public Slider progressBar;
    [SerializeField] private Button claimButton;
    private int key;

    [Header(" Actions ")]
    public static Action<int> onRewardClaimed;

    void Start()
    {
    }

    void Update()
    {

    }

    public void Configure(string title, string rewardString, float progress, int missionIndex)
    {
        titleText.text = title;
        coinText.text = rewardString;
        progressBar.value = progress;
        this.key = missionIndex;

        CheckIfCanClaim(progress);
    }

    // this is so bad, its unreal. could have just used the missionManager, literally the only thing
    // we had to do was "MissionContainer.progressBar.value = value",
    // and "PlayerPrefs = SetFloat().....   xddddddddddddddddddddddddddddddddddddddddddddddddddddddddd             
    public void UpdateProgress(float value)
    {
        progressBar.value = value;
        CheckIfCanClaim(value);
    }

    private void CheckIfCanClaim(float progress)
    {
        if (progress >= 1)
        {
            claimButton.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(false);
        }
    }

    public void Claim()
    {
        onRewardClaimed?.Invoke(key);
    }

    public int GetKey()
    {
        return key;
    }
}
