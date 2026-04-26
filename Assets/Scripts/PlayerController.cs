using System;
using Unity.VisualScripting;
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

    [Header("CheckInteraction Settings")]
    [SerializeField] private float raycastDistance = 2.5f;
    [SerializeField] private LayerMask interactLayer;
    private Vector3 raycastOrigin;
    private Vector3 raycastDirection;
    private Transform rayHit;
    private GameObject currentActiveChild;

    private void Start() {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        PlayerInputManager.Instance.jump += OnJump;
        PlayerInputManager.Instance.interact += OnInteract;
    }


    private void OnDisable() {
        if(PlayerInputManager.Instance != null) {
            PlayerInputManager.Instance.jump -= OnJump;
            PlayerInputManager.Instance.interact -= OnInteract;
        } 
    }

    private void Update() {
        HandlePlayerRotaion();
        MovePlayer();
        CheckInteraction();
    }

    private void OnJump(object sender, EventArgs e) {
        if (groundedPlayer) {
            // Physics formula for jump velocity: sqrt(height * -2 * gravity)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnInteract(object sender, EventArgs e) {
        Debug.Log(rayHit);
    }

    private void CheckInteraction() {
        Transform camTransform = Camera.main.transform;
        raycastOrigin = camTransform.position;
        raycastDirection = camTransform.forward;

        RaycastHit hit;

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, interactLayer)) {
            rayHit = hit.transform;
            if (rayHit.childCount > 0) {
                        GameObject child = rayHit.GetChild(0).gameObject;

                        // Only act if we hit a NEW object
                        if (currentActiveChild != child) {
                            DisableCurrentChild(); // Turn off the old one
                            child.SetActive(true); // Turn on the new one
                            currentActiveChild = child;
                        }
                    }
        } else {
            rayHit = null;
            DisableCurrentChild();
        }
    }

    private void DisableCurrentChild() {
        if (currentActiveChild != null) {
            currentActiveChild.SetActive(false);
            currentActiveChild = null;
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
