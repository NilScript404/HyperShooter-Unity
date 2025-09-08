using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutfitButton : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceContainer;
    [SerializeField] private Button button;

    public void Configure(OutfitData data, bool isUnlocked)
    {
        iconImage.sprite = data.icon;
        priceText.text = data.price.ToString();

        if (isUnlocked)
            priceContainer.SetActive(false);
    }

    public Button GetButton()
    {
        return button;
    }

    public void Unlock()
    {
        priceContainer.SetActive(false);
    }
}
