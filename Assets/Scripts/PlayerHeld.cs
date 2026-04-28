using System;
using UnityEngine;

public class PlayerHeld : MonoBehaviour {
    [Header("Placement Settings")]
    [SerializeField] private bool useGrid = true;
    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform holdSocket;
    private GameObject heldObject;


    private void Start() {
        PlayerInputManager.Instance.action += OnAction;
        PlayerInputManager.Instance.place += OnPlace;
    }

    private void OnDisable() {
        PlayerInputManager.Instance.action -= OnAction;
        PlayerInputManager.Instance.place -= OnPlace;
    }

    private void OnAction(object sender, EventArgs e) {
        
    }


    private void OnPlace(object sender, EventArgs e) {
        if (heldObject == null) return;

        Transform cam = Camera.main.transform;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 10f, groundLayer)) {
            
            Vector3 targetPos = hit.point;
            Debug.Log(targetPos);

            if (useGrid) {
                targetPos.x = Mathf.Round(targetPos.x / gridSize) * gridSize;
                targetPos.z = Mathf.Round(targetPos.z / gridSize) * gridSize;
            }

            Vector3 skyOrigin = new Vector3(targetPos.x, targetPos.y + 5f, targetPos.z);
            if (Physics.Raycast(skyOrigin, Vector3.down, out RaycastHit groundHit, 10f, groundLayer)) {
                PlaceObject(groundHit.point);
            }
        }
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

    private void PlaceObject(Vector3 finalPosition) {
        heldObject.transform.SetParent(null);
        heldObject.transform.position = finalPosition;

        if (heldObject.TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = false;
        }

        heldObject = null;
    }


}
