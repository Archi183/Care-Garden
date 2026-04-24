using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void Start() {
        controller = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        PlayerInputManager.Instance.jump += OnJump;
    }


    private void OnDisable() {
        if(PlayerInputManager.Instance != null)
            PlayerInputManager.Instance.jump -= OnJump;
    }

    private void Update() {
        HandlePlayerRotaion();
        MovePlayer();
    }

    private void OnJump(object sender, EventArgs e) {
        if (groundedPlayer) {
            // Physics formula for jump velocity: sqrt(height * -2 * gravity)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }
    }

    private void MovePlayer() {
        groundedPlayer = controller.isGrounded;

        // Reset downward velocity when touching ground to prevent "force buildup"
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = -2f; // Slight downward force keeps isGrounded reliable
        }

        // 1. Movement relative to player's facing direction
        Vector2 input = PlayerInputManager.Instance.GetPlayerMovement();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        // 2. Gravity application
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandlePlayerRotaion() {
        // Get the camera's forward direction
        Vector3 camForward = Camera.main.transform.forward;

        // Flatten the vector so the player doesn't tilt up/down
        camForward.y = 0;

        // Apply the rotation if there is movement or constant sync is needed
        if (camForward.sqrMagnitude > 0.01f) {
            transform.rotation = Quaternion.LookRotation(camForward);
        }
    }


}
