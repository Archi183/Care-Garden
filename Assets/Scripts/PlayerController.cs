using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 8.0f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravityMuiltiplier = 2f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void Start() {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        PlayerInputManager.Instance.jump += OnJump;
    }


    private void OnDisable() {
        if(PlayerInputManager.Instance != null) {
            PlayerInputManager.Instance.jump -= OnJump;
        } 
    }

    private void Update() {
        HandlePlayerRotaion();
        MovePlayer();
    }

    // Movement Logic
    private void OnJump(object sender, EventArgs e) {
        if (groundedPlayer) {
            // Physics formula for jump velocity: sqrt(height * -2 * gravity)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void MovePlayer() {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }

        Vector2 input = PlayerInputManager.Instance.GetPlayerMovement();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravity * Time.deltaTime * gravityMuiltiplier;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandlePlayerRotaion() {
        Vector3 camForward = Camera.main.transform.forward;

        camForward.y = 0;

        if (camForward.sqrMagnitude > 0.01f) {
            transform.rotation = Quaternion.LookRotation(camForward);
        }
    }

}