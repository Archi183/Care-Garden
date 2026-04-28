using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {
    public event EventHandler jump;
    public event EventHandler interact;
    public event EventHandler action;
    public event EventHandler placeStarted;
    public event EventHandler placeCanceled;
    private PlayerInputAction inputActions;
    public static PlayerInputManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        inputActions = new PlayerInputAction();
    }


    private void OnEnable() {
        inputActions.Enable();
        inputActions.Player.Jump.performed += PlayerJump;
        inputActions.Player.Interact.performed += PlayerInteract;
        inputActions.Player.Action.performed += PlayerAction;
        inputActions.Player.Place.performed += PlayerPlaceStarted;
        inputActions.Player.Place.canceled += PlayerPlaceCanceled;
    }

    private void OnDisable() {
        inputActions.Disable();
        inputActions.Player.Jump.performed -= PlayerJump;
        inputActions.Player.Interact.performed -= PlayerInteract;
        inputActions.Player.Action.performed -= PlayerAction;
        inputActions.Player.Place.performed -= PlayerPlaceStarted;
        inputActions.Player.Place.canceled -= PlayerPlaceCanceled;
    }

    private void PlayerJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        jump?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        interact?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerAction(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        action?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerPlaceStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        placeStarted?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerPlaceCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        placeCanceled?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetPlayerMovement() {
        return inputActions.Player.MOVE.ReadValue<Vector2>();
    }
    public Vector2 GetPlayerLook() {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }

}
