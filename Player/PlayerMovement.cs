using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private CharacterIK playerIK;
    [SerializeField] private CharacterRagdoll characterRagdoll;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowMotion;
    [SerializeField] private Transform enemyTarget;

    [Header(" Actions ")]
    public static Action OnEnterWarzone;
    public static Action OnExitWarzone;
    public static Action onDeath;

    enum State { Idle, Run, Warzone, Dead }
    State state;
    private Warzone currentWarzone;

    [Header(" Spline Settings ")]
    private float warzoneTimer;

    private void Awake()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }


    void Start()
    {
        Application.targetFrameRate = 60;
        state = State.Idle;

        // place the player at the last checkpoint if there were any.
        transform.position = CheckPointManager.instance.GetCheckpointPosition();
    }

    private void ManageState()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Run:
                Move();
                break;
            case State.Warzone:
                ManageWarzoneState();
                break;
        }
    }

    void Update()
    {
        if (GameManager.instance.IsGameState())
            ManageState();
    }

    private void StartRunning()
    {
        state = State.Run;
        playerAnimator.PlayRunAnimation();
    }

    private void Move()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    // this is good, when we enter the warzone, we will only handle the first warzone object,
    // since the colliderdetection is always running, we are only handling the WarzoneEnter tag
    // but then we need to also make sure we are only handling a single warzone ( could use flag too
    // but i guess this is just way better, since we can handle the first Warzone object)
    public void EnterWarzoneCallback(Warzone warzone)
    {
        if (currentWarzone != null)
            return;

        state = State.Warzone;
        currentWarzone = warzone;

        currentWarzone.StartAnimatingIkTarget();

        warzoneTimer = 0;

        playerAnimator.Play(currentWarzone.GetAnimationToPlay(), currentWarzone.GetAnimatorSpeed());

        Time.timeScale = slowMotion;
        // we also need to adjust the timestamp, timestamp is used for physics simulation
        // and if we dont do this, the ragdoll will slow down once the target is sent flying
        Time.fixedDeltaTime = slowMotion / 50; // 50 is the default value for timestamp ???

        playerIK.ConfigureIK(currentWarzone.GetIKTarget());

        OnEnterWarzone?.Invoke();

        Debug.Log("entered the warzone");
    }

    private void ManageWarzoneState()
    {
        // it takes currentWarzone.duration seconds to jump over the warzone
        warzoneTimer += Time.deltaTime;
        // float splinePercent = warzoneTimer / currentWarzone.GetDuration();
        float splinePercent = Mathf.Clamp01(warzoneTimer / currentWarzone.GetDuration());


        transform.position = currentWarzone.GetPlayerSpline().EvaluatePosition(splinePercent);

        if (splinePercent >= 1)
        {
            TryExitWarzone();
        }
    }

    private void TryExitWarzone()
    {
        Warzone nextWarzone = currentWarzone.GetNextWarzone();
        if (nextWarzone == null)
            ExitWarzone();
        else
        {
            currentWarzone = null;
            EnterWarzoneCallback(nextWarzone);
        }
    }

    private void ExitWarzone()
    {
        state = State.Run;
        currentWarzone = null;
        playerAnimator.Play("Run", 1); // any reason why 1 for run speed? just because ?

        OnExitWarzone?.Invoke();

        Time.timeScale = 1;
        Time.fixedDeltaTime = 1f / 50; // in project setting.Time you can see timeStamp and timeScale

        playerIK.DisableIK();
    }

    public Transform GetEnemyTarget()
    {
        return enemyTarget;
    }

    public void TakeDamage()
    {
        state = State.Dead;
        characterRagdoll.Ragdollify();

        Time.timeScale = 1;
        Time.fixedDeltaTime = 1f / 50; // in project setting.Time you can see timeStamp and timeScale

        GameManager.instance.SetGameState(GameState.GameOver);
        onDeath?.Invoke();
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                StartRunning();
                break;
        }
    }

    public void HitFinishLine()
    {
        state = State.Idle;
        playerAnimator.PlayIdleAnimation();

        GameManager.instance.SetGameState(GameState.LevelComplete);
    }

    public Warzone GetCurrentWarzone()
    {
        return currentWarzone;
    }
}
