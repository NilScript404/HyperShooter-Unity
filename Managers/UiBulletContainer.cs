using UnityEngine;
using UnityEngine.UI;

public class UiBulletContainer : MonoBehaviour
{
    public static UiBulletContainer instance;

    [Header(" Elements ")]
    [SerializeField] private Transform bulletsParent;

    [Header(" Colors ")]
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    private int bulletsShot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        PlayerShooter.onShot += OnShotCallback;
        PlayerMovement.OnEnterWarzone += OnEnteredWarzoneCallback;
        PlayerMovement.OnExitWarzone += OnExitedWarzoneCallback;
    }

    private void OnDestroy()
    {
        PlayerShooter.onShot -= OnShotCallback;
        PlayerMovement.OnEnterWarzone -= OnEnteredWarzoneCallback;
        PlayerMovement.OnExitWarzone -= OnExitedWarzoneCallback;
    }

    void Start()
    {
        bulletsParent.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    private void OnShotCallback()
    {
        bulletsShot++;
        bulletsShot = Mathf.Min(bulletsShot, bulletsParent.childCount);

        bulletsParent.GetChild(bulletsShot - 1).GetComponent<Image>().color = inactiveColor;
    }

    public void OnExitedWarzoneCallback()
    {
        bulletsParent.gameObject.SetActive(false);
        Reload();
    }

    public void OnEnteredWarzoneCallback()
    {
        bulletsParent.gameObject.SetActive(true);
    }

    public bool CanShoot()
    {
        return bulletsShot < bulletsParent.childCount;
    }

    public void Reload()
    {
        for (int i = 0; i < bulletsShot; i++)
        {
            bulletsParent.GetChild(i).GetComponent<Image>().color = activeColor;
        }
        bulletsShot = 0;
    }
}
