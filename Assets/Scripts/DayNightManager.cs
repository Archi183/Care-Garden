using System;
using UnityEngine;

public class DayNightManager : MonoBehaviour {
    public event EventHandler dayEnd; 
    [SerializeField] private float perDayTimeMin = 20f;
    private Light sun;
    
    [Header("Clock Settings")]
    [SerializeField] private float currentTime = 0.25f;
    private float currentDay;

    private void Start() {
        sun = RenderSettings.sun;
        if (sun != null) {
            Debug.Log("Sun source found: " + sun.name);  
        }

    }   

    private void Update() {
        if (sun != null) {
            UpdateClock();
            UpdateSun();
        }
    }
    // Clock logic
    private void UpdateClock() {

        float dayLengthInSeconds = perDayTimeMin * 60f;
        currentTime += Time.deltaTime / dayLengthInSeconds;

        // 2. Check for New Day
        if (currentTime >= 1f) {
            currentTime = 0f;
            currentDay++;
            Debug.Log("Day " + currentDay + " has started!");
            // This is where you will eventually tell your plants to grow!
            dayEnd?.Invoke(this, EventArgs.Empty);
        }

        float totalHours = currentTime * 24f;
        int hours = Mathf.FloorToInt(totalHours);
        int minutes = Mathf.FloorToInt((totalHours * 60f) % 60f);

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        Debug.Log("Current Time: " + timeString);

    }

    // Sun rotation logic
    private void UpdateSun() {
        float sunAngle = (currentTime * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 270f, 0f);
    }
}
