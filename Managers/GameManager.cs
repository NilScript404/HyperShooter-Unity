using UnityEngine;
using System;
using UnityEngine.SceneManagement;

// now we can work with GameState like a shared state variable, without having to write something like
// GameState.Menu, GameState.Game, etc ...
// just to make things a bit easier is all otherwise it would have been fine too.
public enum GameState { Menu, Game, LevelComplete, GameOver }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header(" Settings ")]
    private GameState gameState;

    [Header(" Actions ")]
    public static Action<GameState> onGameStateChanged;

    // it would be good if GameManager was a singleton 
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetGameState(GameState.Menu);
    }

    void Update()
    {

    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        // will tell the level manager to increase the level index later on.
        SceneManager.LoadScene(0);
    }
}
