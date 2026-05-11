using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // ! === Game Manager ===
    private GameManager gameManager;

    [Header("Toggle-ables")]
    [Tooltip("If the user is currently interacting with this object")]
    public bool interacting = false;
    public State activeOnState;

    public void SetInteracting(bool value) {
        interacting = value;
    }

    
    // ? --- Move Up --- //
    public event Action OnUp;
    private void HandleUp()
    {
        if (gameManager.state != activeOnState) return;
        OnUp?.Invoke();
    }
    
    // ? --- Move Right --- //
    public event Action OnRight;
    private void HandleRight()
    {
        if (gameManager.state != activeOnState) return;
        OnRight?.Invoke();
    }

    // ? --- Move Down --- //
    public event Action OnDown;
    private void HandleDown()
    {
        if (gameManager.state != activeOnState) return;
        OnDown?.Invoke();
    }

    // ? --- Move Left --- //
    public event Action OnLeft;
    private void HandleLeft()
    {
        if (gameManager.state != activeOnState) return;
        OnLeft?.Invoke();
    }

    // ? --- Select --- //
    public event Action OnSelect;
    private void HandleSelect()
    {
        if (gameManager.state != activeOnState) return;
        OnSelect?.Invoke();
    }

    // ? --- Light --- //
    public event Action OnLight;
    private void HandleLight()
    {
        if (gameManager.state != activeOnState) return;
        OnLight?.Invoke();
    }

    // ? --- Heavy --- //
    public event Action OnHeavy;
    private void HandleHeavy()
    {
        if (gameManager.state != activeOnState) return;
        OnHeavy?.Invoke();
    }

    // ? --- Special --- //

    // ! === Base Listeners === //
    void Awake()
    {
        this.gameManager = FindFirstObjectByType<GameManager>();

        gameManager.onMoveUp.AddListener(HandleUp);
        gameManager.onMoveRight.AddListener(HandleRight);
        gameManager.onMoveDown.AddListener(HandleDown);
        gameManager.onMoveLeft.AddListener(HandleLeft);
        gameManager.onSelect.AddListener(HandleSelect);
        gameManager.onLight.AddListener(HandleLight);
        gameManager.onHeavy.AddListener(HandleHeavy);
    }

}
