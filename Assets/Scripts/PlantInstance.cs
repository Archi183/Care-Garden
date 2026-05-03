using System;
using UnityEngine;

public class PlantInstance : MonoBehaviour {
    public PlantSO plantData;
    private int currentAge = 0;
    private GameObject spawnedPlant;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Start() {
        spawnPosition = Vector3.zero;
        float randomY = UnityEngine.Random.Range(0f, 360f);
        spawnRotation = Quaternion.Euler(0, randomY, 0);
        DayNightManager.Instance.dayEnd += OnDayEndPlant;
        spawnedPlant = Instantiate(plantData.growthStages[currentAge], spawnPosition, spawnRotation, transform);
        spawnedPlant.transform.localPosition = spawnPosition; 
        spawnedPlant.transform.localRotation = spawnRotation;
    }
    private void OnDisable() {
        DayNightManager.Instance.dayEnd -= OnDayEndPlant;
    }

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

}
