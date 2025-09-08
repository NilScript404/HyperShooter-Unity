using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;

    [Header(" Data ")]
    private int coins;

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI[] coinTexts;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        LoadData();
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            AddCoins(1000);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveData();
    }

    public bool CanPurchase(int amount)
    {
        return coins >= amount;
    }

    public void Purchase(int amount)
    {
        coins -= amount;
        SaveData();
    }

    public void UpdateTexts()
    {
        foreach (TextMeshProUGUI coinText in coinTexts)
            coinText.text = coins.ToString();
    }

    public void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins", 100);
        UpdateTexts();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Counts",coins );
        UpdateTexts();
    }
}
