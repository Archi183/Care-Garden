using System;
using NUnit.Framework;
using UnityEngine;

public class PlayerHeld : MonoBehaviour {
    public static PlayerHeld Instance { get; private set; }
    [Header("Placement Settings")]
    [SerializeField] private bool useGrid = true;
    [SerializeField] private float gridSizeMultiplier = 2f;
    [SerializeField] private float gridSize = 0.18f;
    [SerializeField] private float rayDistFromPLayer = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int plantPlacedLayer = 6;
    [SerializeField] private Transform holdSocket;
    private bool isPlacing = false;
    private GameObject heldObject;
    private Collider objCol;
    private Rigidbody objRB;
    private Vector3 currentPreviewPosition;
    private Quaternion currentPreviewRotation;
    private GameObject groundGrid;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        currentPreviewRotation = Quaternion.identity;
        groundGrid = GameObject.Find("GardenGroundGrid");
        groundGrid.SetActive(false);
        PlayerInputManager.Instance.interact += ToggleGrid;
        PlayerInputManager.Instance.actionStarted += OnActionStarted;
        PlayerInputManager.Instance.actionCanceled += OnPlaceCanceled;
        PlayerInputManager.Instance.placeStarted += OnPlaceStarted;
        PlayerInputManager.Instance.placeCanceled += OnPlaceCanceled;
        PlayerInputManager.Instance.clockScroll += ClockScroll;
        PlayerInputManager.Instance.aniticlockScroll += AnticlockScroll;
    }

    private void OnDisable() {
        PlayerInputManager.Instance.interact -= ToggleGrid;
        PlayerInputManager.Instance.actionStarted -= OnActionStarted;
        PlayerInputManager.Instance.actionCanceled -= OnPlaceCanceled;
        PlayerInputManager.Instance.placeStarted -= OnPlaceStarted;
        PlayerInputManager.Instance.placeCanceled -= OnPlaceCanceled;
        PlayerInputManager.Instance.clockScroll -= ClockScroll;
        PlayerInputManager.Instance.aniticlockScroll -= AnticlockScroll;
    }

    private void Update() {
        Debug.Log(heldObject);
        if(objCol) Debug.Log("Found child-col");
        if(objRB) Debug.Log("Found child-rb");
        OnPlaceUpdatePreview();
    }

    private void ToggleGrid(object sender, EventArgs e) {
        if (heldObject != null && isPlacing) {
            useGrid = !useGrid;
            groundGrid.SetActive(useGrid);
        }
        
    }
    private void ClockScroll(object sender, EventArgs e) {
        if (heldObject != null && isPlacing) {
            currentPreviewRotation *= Quaternion.Euler(0, 90, 0);
            Debug.Log("Scrolling!");
        }
    }

    private void AnticlockScroll(object sender, EventArgs e) {
        if (heldObject != null && isPlacing) {
            currentPreviewRotation *= Quaternion.Euler(0, -90, 0);
        }
    }

    private void OnPlaceStarted(object sender, EventArgs e) {
        if (heldObject == null) return;
        isPlacing = true;
        groundGrid.SetActive(true);
        
        
        if (objCol != null) {
            objCol.enabled = true;
        }
    }

    private void OnPlaceCanceled(object sender, EventArgs e) {
        if (!isPlacing || heldObject == null) return;
        PlaceObject(currentPreviewPosition);
        isPlacing = false;
        groundGrid.SetActive(false);
        if (objCol != null) {
            objCol.enabled = true;
            objCol = null;
        }
        if (objRB != null) {
            objRB.isKinematic = false;
            objRB = null;
        }
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
            heldObject.transform.rotation = currentPreviewRotation;

        }

    }

    private void OnActionStarted(object sender, EventArgs e) {
        if (heldObject == null) return;
        isPlacing = true;
        groundGrid.SetActive(true);

        if (objCol != null) {
            objCol.enabled = true;
        }

        SetLayerRecursively(heldObject, plantPlacedLayer);
    }

    private void PlaceObject(Vector3 finalPosition) {
        heldObject.transform.SetParent(null);
        heldObject.transform.position = finalPosition;

        if (objRB != null) {
            objRB.isKinematic = false;
        }

        heldObject = null;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer) {
        if (obj == null) return;

        obj.layer = newLayer;
        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public bool HasObject() {
        return heldObject != null;
    }

    public void PickUp(GameObject obj) {
        heldObject = obj;
        
        objRB = obj.GetComponent<Rigidbody>();
        if (objRB == null) {
            objRB = obj.GetComponent<Rigidbody>();
        }

        objCol = obj.GetComponent<BoxCollider>();
        if (objCol == null) {
            objCol = obj.GetComponent<Collider>();
        }

        // Disable physics so it doesn't fight the player
        if (objRB != null) {
            objRB.linearVelocity = Vector3.zero;
            objRB.angularVelocity = Vector3.zero;
            objRB.isKinematic = true;
        }

        if (objCol != null) {
            objCol.enabled = false;
        }

        // Snap to the hand socket
        heldObject.transform.SetParent(holdSocket);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

}
