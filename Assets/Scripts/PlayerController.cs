using System;
using UnityEditor.Animations;
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
    [SerializeField] private Transform held;
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

    // Check Interact Logic
    private void OnInteract(object sender, EventArgs e) {
        Debug.Log(rayHit.root);
        rayHit.root.position = Vector3.zero;
        rayHit.root.SetParent(held, false);
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

                        if (currentActiveChild != child) {
                            DisableCurrentChild();
                            child.SetActive(true);
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



}