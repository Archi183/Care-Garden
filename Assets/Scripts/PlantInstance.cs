using System;
using UnityEngine;

public class PlantInstance : MonoBehaviour {
    public PlantSO plantData;
    private int currentAge = 0;
    private GameObject spawnedPlant;
    private Transform canSpawnPlant;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    [SerializeField] private Material canPlace;
    [SerializeField] private Material canNotPlace;
    [SerializeField] private LayerMask targetMask;

    private void Start() {
        spawnPosition = Vector3.zero;
        float randomY = UnityEngine.Random.Range(0f, 360f);
        spawnRotation = Quaternion.Euler(0, randomY, 0);
        DayNightManager.Instance.dayEnd += OnDayEndPlant;
        spawnedPlant = Instantiate(plantData.growthStages[currentAge], spawnPosition, spawnRotation, transform);
        spawnedPlant.transform.localPosition = spawnPosition; 
        spawnedPlant.transform.localRotation = spawnRotation;
        canSpawnPlant = transform.Find("Placable");
        canSpawnPlant.gameObject.SetActive(false);

        PlayerInputManager.Instance.actionStarted += OnPlacementStart;
        PlayerInputManager.Instance.actionCanceled += OnPlacementEnd;
        PlayerInputManager.Instance.placeStarted += OnPlacementStart;
        PlayerInputManager.Instance.placeCanceled += OnPlacementEnd;
    }
    private void OnDisable() {
        DayNightManager.Instance.dayEnd -= OnDayEndPlant;
        PlayerInputManager.Instance.actionStarted -= OnPlacementStart;
        PlayerInputManager.Instance.actionCanceled -= OnPlacementEnd;
        PlayerInputManager.Instance.placeStarted -= OnPlacementStart;
        PlayerInputManager.Instance.placeCanceled -= OnPlacementEnd;
    }

    // Plant responce to day logic
    private void OnDayEndPlant(object sender, EventArgs e) {
        if (spawnedPlant != null) {
            currentAge++;
            if (currentAge >= plantData.daysToGrow) return;
            UpdatePrefab();
        } else {
            Debug.Log("No plant prefab found!");
        }
    }

    private void UpdatePrefab() {
        Destroy(spawnedPlant); // Removes it from the scene
        spawnedPlant = Instantiate(plantData.growthStages[currentAge], spawnPosition, spawnRotation, transform);
        spawnedPlant.transform.localPosition = spawnPosition; 
        spawnedPlant.transform.localRotation = spawnRotation;
    }

    // can place logic
    private void OnPlacementStart(object sender, EventArgs e) {
        if (PlayerHeld.Instance.HasObject()) {
            canSpawnPlant.gameObject.SetActive(true);
        }
    }

    private void OnPlacementEnd(object sender, EventArgs e) {
        canSpawnPlant.gameObject.SetActive(false);  
    }

    private void OnCollisionEnter(Collision collision) {
        if (((1 << collision.gameObject.layer) & targetMask) != 0) {
            Debug.Log("Hit something in the mask!");
            canSpawnPlant.GetComponent<Renderer>().material = canNotPlace;
        }
    }


}
