using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    [SerializeField] private float globalCurrentDay = 1;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        DayNightManager.Instance.dayEnd += OnDayEnd;
    }

    private void OnDisable() {
        DayNightManager.Instance.dayEnd -= OnDayEnd;
    }

    private void OnDayEnd(object sender, EventArgs e) {
        globalCurrentDay++;
        Debug.Log("Day " + globalCurrentDay + " has started!");
    }
}
