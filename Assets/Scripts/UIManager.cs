using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private int Round = 0;
    public bool isPaused = false;

    private TMP_Text waveCountText;
    private TMP_Text timeText;

    // Start is called before the first frame update
    void Awake()
    {
        waveCountText = transform.Find("WaveCount").gameObject.GetComponent<TMP_Text>();
        timeText = transform.Find("TimeDisplay").gameObject.GetComponent<TMP_Text>();
        waveCountText.text = "wave: " + Round;
    }

    public void UpdateTimeDisplay(float counter)
    {
        int minutes = (int)(counter / 60);
        int seconds = (int)(counter % 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = "Time: " + timeString;
    }

    public void UpdateWaveDisplay(int waveNumber)
    {
        waveCountText.text = "Wave: " + waveNumber;
    }
}
