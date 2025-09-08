using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct OutfitData
{
    public Mesh mesh;
    public int price;
    public Sprite icon;
}

public class OutfitShopManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private OutfitButton outfitButtonPrefab;
    [SerializeField] private Transform outfitButtonsParent;
    [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer outfitPlayerSkinnedMeshRenderer;
    [SerializeField] private Button purchaseButton;

    private int lastOutfitIndex;
    private int clickedOutfitIndex;

    [Header(" Data ")]
    [SerializeField] private OutfitData[] outfitDatas;

    [Header(" Actions ")]
    public static Action onOutfitUnlocked;

    void Start()
    {
        // loading and applying the last outfit
        LoadLastOutfit();
        playerSkinnedMeshRenderer.sharedMesh = outfitDatas[lastOutfitIndex].mesh;
        outfitPlayerSkinnedMeshRenderer.sharedMesh = outfitDatas[lastOutfitIndex].mesh;

        SaveOutfitState(0);

        CreateOutfitButtons();
    }

    void Update()
    {

    }

    private void CreateOutfitButtons()
    {
        for (int i = 0; i < outfitDatas.Length; i++)
        {
            CreateOutfitButton(i);
        }
    }

    private void CreateOutfitButton(int index)
    {
        bool isUnlocked = IsOutfitUnlocked(index);

        OutfitButton buttonInstance = Instantiate(outfitButtonPrefab, outfitButtonsParent);
        buttonInstance.Configure(outfitDatas[index], isUnlocked);

        buttonInstance.GetButton().onClick.AddListener(() => OutfitButtonCallback(index));
    }

    private void OutfitButtonCallback(int index)
    {
        // outfit viewer
        outfitPlayerSkinnedMeshRenderer.sharedMesh = outfitDatas[index].mesh;

        if (IsOutfitUnlocked(index))
        {
            playerSkinnedMeshRenderer.sharedMesh = outfitDatas[index].mesh;

            // saving the selected outfit as the last selected outfit, so if we restart the game we are
            // still going to be using the last selected outfit
            lastOutfitIndex = index;
            SaveLastOutfit();
        }

        // manage the ui state of the purchase button 
        purchaseButton.interactable = CashManager.instance.CanPurchase(outfitDatas[index].price) && !IsOutfitUnlocked(index);

        // storing which button is selected already, so we can use correctly find its price and buy it, or anything else
        clickedOutfitIndex = index;
    }

    public void Purchase()
    {
        // remove the coin from our pocket
        CashManager.instance.Purchase(outfitDatas[clickedOutfitIndex].price);

        // unlock the ui of the button of our selected skin
        outfitButtonsParent.GetChild(clickedOutfitIndex).GetComponent<OutfitButton>().Unlock();

        // save the state of that skin, so its actually bought and saved. 
        SaveOutfitState(clickedOutfitIndex);

        // updating the state of purchase button after we have bought the skin
        purchaseButton.interactable = false;

        // actually change the mesh of our player, now the skin is applied to our player and we are done.
        playerSkinnedMeshRenderer.sharedMesh = outfitDatas[clickedOutfitIndex].mesh;

        // saving the selected outfit as the last selected outfit, so if we restart the game we are
        // still going to be using the last selected outfit
        lastOutfitIndex = clickedOutfitIndex;
        SaveLastOutfit();

        onOutfitUnlocked?.Invoke();
    }

    private bool IsOutfitUnlocked(int index)
    {
        return PlayerPrefs.GetInt("Outfit" + index) == 1;
    }

    private void SaveOutfitState(int index)
    {
        PlayerPrefs.SetInt("Outfit" + index, 1);
    }

    private void LoadLastOutfit()
    {
        lastOutfitIndex = PlayerPrefs.GetInt("lastOutfit");
        playerSkinnedMeshRenderer.sharedMesh = outfitDatas[lastOutfitIndex].mesh;
    }

    private void SaveLastOutfit()
    {
        PlayerPrefs.SetInt("lastOutfit", lastOutfitIndex);
    }
}
