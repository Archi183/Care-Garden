using UnityEngine;

[CreateAssetMenu(fileName = "PLantSO", menuName = "ScriptableObjects/PLant")]
public class PlantSO : ScriptableObject {
    public string plantName;
    public int daysToGrow;
    public GameObject[] growthStages;
}