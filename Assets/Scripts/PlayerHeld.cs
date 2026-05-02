using System;
using UnityEngine;

public class PlayerHeld : MonoBehaviour {
    [Header("Placement Settings")]
    [SerializeField] private bool useGrid = true;
    [SerializeField] private float gridSizeMultiplier = 2f;
    [SerializeField] private float gridSize = 0.18f;
    [SerializeField] private float rayDistFromPLayer = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform holdSocket;
    private bool isPlacing = false;
    private GameObject heldObject;
    private Vector3 currentPreviewPosition;
    private GameObject groundGrid;

    private void Start() {
        groundGrid = GameObject.Find("GardenGroundGrid");
        groundGrid.SetActive(false);
        PlayerInputManager.Instance.interact += ToggleGrid;
        PlayerInputManager.Instance.action += OnAction;
        PlayerInputManager.Instance.placeStarted += OnPlaceStarted;
        PlayerInputManager.Instance.placeCanceled += OnPlaceCanceled;
    }

    private void OnDisable() {
        PlayerInputManager.Instance.interact -= ToggleGrid;
        PlayerInputManager.Instance.action -= OnAction;
        PlayerInputManager.Instance.placeStarted -= OnPlaceStarted;
        PlayerInputManager.Instance.placeCanceled -= OnPlaceCanceled;
    }

    private void Update() {
        OnPlaceUpdatePreview();
    }

    private void ToggleGrid(object sender, EventArgs e) {
        if (heldObject != null && isPlacing) {
            useGrid = !useGrid;
            groundGrid.SetActive(useGrid);
        }
        
    }
    private void OnPlaceStarted(object sender, EventArgs e) {
        if (heldObject == null) return;
        isPlacing = true;
        groundGrid.SetActive(true);
    }

    private void OnPlaceCanceled(object sender, EventArgs e) {
        if (!isPlacing || heldObject == null) return;
        PlaceObject(currentPreviewPosition);
        isPlacing = false;
        groundGrid.SetActive(false);
    }

    private void OnPlaceUpdatePreview() {
        float gridValue = gridSize * gridSizeMultiplier;
        float rayStartHight = 4f;
        float placingRaycastDepth = 10f;
        if (!isPlacing || heldObject == null) return;

        Transform cam = Camera.main.transform;

        Vector3 placingRaycastOrigin = cam.position + cam.forward * rayDistFromPLayer;
        placingRaycastOrigin.y += rayStartHight;

        if (Physics.Raycast(placingRaycastOrigin, Vector3.down, out RaycastHit hit, placingRaycastDepth, groundLayer)) {
            currentPreviewPosition = hit.point;

            if (useGrid) {
                currentPreviewPosition.x = Mathf.Round(currentPreviewPosition.x / gridValue) * gridValue;
                currentPreviewPosition.z = Mathf.Round(currentPreviewPosition.z / gridValue) * gridValue;
            }

            heldObject.transform.position = currentPreviewPosition;
        }

    }

    private void OnAction(object sender, EventArgs e) {
        
    }

    private void PlaceObject(Vector3 finalPosition) {
        heldObject.transform.SetParent(null);
        heldObject.transform.position = finalPosition;

        if (heldObject.TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = false;
        }

        heldObject = null;
    }

    public bool HasObject() {
        return heldObject != null;
    }

    public void PickUp(GameObject obj) {
        heldObject = obj;
        
        // Disable physics so it doesn't fight the player
        if (heldObject.TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = true;
        }

        // Snap to the hand socket
        heldObject.transform.SetParent(holdSocket);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

}
