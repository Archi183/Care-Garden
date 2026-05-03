using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private float globalCurrentDay = 1;

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
