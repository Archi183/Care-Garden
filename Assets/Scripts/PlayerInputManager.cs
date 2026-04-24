using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {
    public event EventHandler jump;
    private PlayerInputAction inputActions;
    public static PlayerInputManager Instance { get; private set; }

    private void Awake() {
        // Check if an instance already exists
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        Instance = this;
        
        // Optional: Keep this object alive across different scenes
        DontDestroyOnLoad(gameObject);
        inputActions = new PlayerInputAction();
    }


    private void OnEnable() {
        inputActions.Enable();
        inputActions.Player.Jump.performed += PlayerJump;
    }

    private void OnDisable() {
        inputActions.Disable();
        inputActions.Player.Jump.performed -= PlayerJump;
    }

    private void PlayerJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        jump?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetPlayerMovement() {
        return inputActions.Player.MOVE.ReadValue<Vector2>();
    }
    public Vector2 GetPlayerLook() {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }

}
