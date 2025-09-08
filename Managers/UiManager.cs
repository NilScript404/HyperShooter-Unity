using UnityEngine;

public class UiManager : MonoBehaviour
{

    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject outfitShopPanel;
    [SerializeField] private GameObject missionPanel;

    public void Awake()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    public void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    void Start()
    {
    }

    void Update()
    {

    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                levelCompletePanel.SetActive(false);
                gameOverPanel.SetActive(false);

                CloseMissionPanel();
                CloseOutfitShop();
                break;
            case GameState.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                break;
            case GameState.GameOver:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameOverPanel.SetActive(true);
                break;
            case GameState.LevelComplete:
                gamePanel.SetActive(false);
                levelCompletePanel.SetActive(true);
                break;
        }
    }

    public void PlayButtonCallback()
    {
        GameManager.instance.SetGameState(GameState.Game);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void RetryButtonCallback()
    {
        GameManager.instance.Retry();
    }

    public void NextButtonCallback()
    {
        GameManager.instance.NextLevel();
    }

    public void OpenOutfitShop()
    {
        outfitShopPanel.SetActive(true);
    }

    public void CloseOutfitShop()
    {
        outfitShopPanel.SetActive(false);
    }

    public void OpenMissionPanel()
    {
        missionPanel.SetActive(true);
    }

    public void CloseMissionPanel()
    {
        missionPanel.SetActive(false);
    }
}