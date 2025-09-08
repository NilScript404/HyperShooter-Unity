using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    [Header(" Settings ")]
    private Vector3 lastCheckPointPosition;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // because if we dont do this, then after reloading the Scene[0], the lastCheckPointPosition 
        // will be reset too, so we cant store our previous checkPointPosition.
        DontDestroyOnLoad(this);

        CheckPoint.onInteracted += CheckPointInteractedCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    public void OnDestroy()
    {
        CheckPoint.onInteracted -= CheckPointInteractedCallback;
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
            case GameState.LevelComplete:
                lastCheckPointPosition = new Vector3((float)-2.74, 0 , (float)-0.18);
                break;
        }
    }

    public void CheckPointInteractedCallback(CheckPoint checkpoint)
    {
        lastCheckPointPosition = checkpoint.GetPosition();
    }

    public Vector3 GetCheckpointPosition()
    {
        return lastCheckPointPosition;
    }
}
