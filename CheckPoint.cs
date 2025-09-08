using UnityEngine;
using System;

public class CheckPoint : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private SpriteRenderer gradient;

    [Header(" Actions ")]
    public static Action<CheckPoint> onInteracted;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Interact()
    {
        gradient.color = Color.red;

        onInteracted?.Invoke(this);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
