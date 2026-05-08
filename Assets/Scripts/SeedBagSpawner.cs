using UnityEngine;

public class SeedBagSpawner : MonoBehaviour {
    [SerializeField] private int numberOfBoxes = 2;
    [SerializeField] private GameObject seedBox;
    [SerializeField] private float spawnHeight = 4f;
    
    private MeshRenderer planeRenderer;

    private void Awake() {
        planeRenderer = GetComponent<MeshRenderer>();

        if (planeRenderer != null) {
            planeRenderer.enabled = false; 
        }
    }

    private void Start() {
        for (int i = 0; i < numberOfBoxes; i++) {
            SpawnSeedBag();
        }
    }

    private void SpawnSeedBag() {
        if (planeRenderer == null) return;

        Bounds bounds = planeRenderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 spawnPos = new Vector3(randomX, spawnHeight, randomZ);
        Instantiate(seedBox, spawnPos, Quaternion.identity);
    }

}
