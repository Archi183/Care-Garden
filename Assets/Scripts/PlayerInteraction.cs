using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    [Header("CheckInteraction Settings")]
    [SerializeField] private float raycastDistance = 2.5f;
    [SerializeField] private LayerMask interactLayer;
    private Vector3 raycastOrigin;
    private Vector3 raycastDirection;
    private Transform rayHit;
    private GameObject currentActiveChild;
    [SerializeField] private PlayerHeld playerHeld;


    private void Start() {
        PlayerInputManager.Instance.interact += OnInteract;
    }

    private void OnDisable() {
        PlayerInputManager.Instance.interact -= OnInteract;
    }

    private void Update() {
        CheckInteraction();
    }
    

    // Check Interact Logic
    private void OnInteract(object sender, EventArgs e) {
        if (rayHit != null && !playerHeld.HasObject()) {
            Debug.Log(rayHit.root);
            playerHeld.PickUp(rayHit.root.gameObject);
        } else {
          return;  
        }
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