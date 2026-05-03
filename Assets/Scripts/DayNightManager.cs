using UnityEngine;

public class DayNightManager : MonoBehaviour {
    [SerializeField] private float perDayTimeMin = 20f;
    private Light sun;
    private float rotationDegree;
    private float fullRotationDegree = 360f;
    private void Start() {
        sun = RenderSettings.sun;
        if (sun != null) {
            Debug.Log("Sun source found: " + sun.name);
            sun.transform.rotation = Quaternion.Euler(0, 270, 0);    
        }
        float perDayTimeSec = perDayTimeMin * 60f;
        rotationDegree = fullRotationDegree / perDayTimeSec;

    }   

    private void Update() {
        if (sun != null) {
            RotateSun();
        }
    }
    // Sun rotatio logic
    private void RotateSun() {
        sun.transform.Rotate(Vector3.right, rotationDegree * Time.deltaTime);
    }
}
