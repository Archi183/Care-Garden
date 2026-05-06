using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {
    public event EventHandler runStarted;
    public event EventHandler runCancelled;
    public event EventHandler jump;
    public event EventHandler interact;
    public event EventHandler actionStarted;
    public event EventHandler actionCanceled;
    public event EventHandler placeStarted;
    public event EventHandler placeCanceled;
    public event EventHandler clockScroll;
    public event EventHandler aniticlockScroll;
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
        inputActions.Player.Run.performed += PlayerRunStarted;
        inputActions.Player.Run.canceled += PlayerRunCancelled;
        inputActions.Player.Jump.performed += PlayerJump;
        inputActions.Player.Interact.performed += PlayerInteract;
        inputActions.Player.Action.performed += PlayerActionStarted;
        inputActions.Player.Action.canceled += PlayerActionCanceled;
        inputActions.Player.Place.performed += PlayerPlaceStarted;
        inputActions.Player.Place.canceled += PlayerPlaceCanceled;
        inputActions.Player.Scroll.performed += PlayerScroll;
    }

    private void OnDisable() {
        inputActions.Disable();
        inputActions.Player.Run.performed -= PlayerRunStarted;
        inputActions.Player.Run.canceled -= PlayerRunCancelled;
        inputActions.Player.Jump.performed -= PlayerJump;
        inputActions.Player.Interact.performed -= PlayerInteract;
        inputActions.Player.Action.performed -= PlayerActionStarted;
        inputActions.Player.Action.canceled -= PlayerActionCanceled;
        inputActions.Player.Place.performed -= PlayerPlaceStarted;
        inputActions.Player.Place.canceled -= PlayerPlaceCanceled;
        inputActions.Player.Scroll.performed -= PlayerScroll;
    }

    private void PlayerScroll(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        Vector2 scrollVector = obj.ReadValue<Vector2>();

        if (scrollVector.y > 0) {
            // Scrolled UP (Clockwise/Next)
            clockScroll?.Invoke(this, EventArgs.Empty);
        } 
        else if (scrollVector.y < 0) {
            // Scrolled DOWN (Anti-Clockwise/Previous)
            aniticlockScroll?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PlayerRunStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        runStarted?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerRunCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        runCancelled?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        jump?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        interact?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerActionStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        actionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        actionCanceled?.Invoke(this, EventArgs.Empty);
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
