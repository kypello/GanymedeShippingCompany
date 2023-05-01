using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time = 180f;
    public TMP_Text display;
    public bool timing = false;

    void Start() {
        Display();
    }

    void Update() {
        if (timing) {
            time -= Time.deltaTime;
            if (time <= 0f) {
                time = 0f;
                timing = false;
            }
            Display();
        }
    }

    public void StartTiming() {
        timing = true;
    }

    void Display() {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        if (seconds >= 10) {

            display.text = "<mspace=30>" + minutes + "</mspace>:<mspace=30>" + seconds;
        }
        else {
            display.text = "" + minutes + "</mspace>:<mspace=30>0" + seconds;
        }
    }
}
