using System;
using UnityEngine;

public class DayNightManager : MonoBehaviour {
    public static DayNightManager Instance { get; private set; }

    public event EventHandler dayEnd; 
    [SerializeField] private float perDayTimeMin = 20f;
    private Light sun;
    private float currentSunAngle;
    
    [Header("Clock Settings")]
    [SerializeField] private float currentTime = 0.25f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        sun = RenderSettings.sun;

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
            // This is where you will eventually tell your plants to grow!
            dayEnd?.Invoke(this, EventArgs.Empty);
        }

        float totalHours = currentTime * 24f;
        int hours = Mathf.FloorToInt(totalHours);
        int minutes = Mathf.FloorToInt((totalHours * 60f) % 60f);

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        // Debug.Log("Current Time: " + timeString);

    }

    // Sun rotation logic
    private void UpdateSun() {
        float percentRotation = 5f;
        float sunAngle = (currentTime * 360f) - 90f;
        Quaternion targetRotation = Quaternion.Euler(sunAngle, 270f, 0f);
        sun.transform.rotation = Quaternion.Slerp(sun.transform.rotation, targetRotation, Time.deltaTime * percentRotation);
    }

}
